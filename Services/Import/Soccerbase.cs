using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using FootballOracle.Models.ViewModels.Approvable.Matches;

namespace FootballOracle.Services.Import
{
    public class Soccerbase : BaseImport
    {
        public Soccerbase(IRepositoryProvider provider, IPrincipal user) 
            : base(provider, ImportSite.Soccerbase, user)
        {
        }

        private enum ImportType 
        { 
            Default,
            Referee, 
            Attendance,
            Venue
        }

        public async Task Import(DateTime startDate, DateTime endDate)
        {
            // http://www.soccerbase.com/matches/results.sd?date=2014-08-18

            for (var importDate = startDate; importDate <= endDate; importDate = importDate.AddDays(1))
            {
                var uri = new Uri(string.Format("http://www.soccerbase.com/matches/results.sd?date={0}", importDate.ToString("yyyy-MM-dd")));
                var document = GetHtmlDocument(uri);

                var table = document.DocumentNode.Descendants("table").Where(tb => tb.Attributes.Contains("class") && tb.Attributes["class"].Value == "soccerGrid listWithCards").FirstOrDefault();

                if (table == null)
                    continue;

                Competition competition = null;
                var competitionName = string.Empty;
                bool isCompetitionValid = true;
                Guid? countryKey = null;

                foreach(var row in table.Descendants("tr"))
                {
                    var div = row.Descendants("div").FirstOrDefault();

                    //Competition
                    if (div != null && div.Attributes["class"].Value == "headlineBlock")
                    {
                        if (!div.Descendants("a").Any())
                        {
                            isCompetitionValid = false;
                            continue;
                        }

                        isCompetitionValid = true;
                        var href = div.Descendants("a").First().Attributes["href"].Value;
                        var lookupCompetitionId = href.GetTextAfter("tournament.sd?comp_id=");
                        competitionName = div.Descendants("a").First().InnerText;
                        competition = await LookupCompetition(lookupCompetitionId);

                        if (competition != null)
                        {
                            var competitionViewModel = (await Provider.GetCompetition((Guid)competition.PrimaryKey, importDate)).ToViewModel(importDate);
                            var country = competitionViewModel.HeaderEntity.GetParentCountry(importDate);
                            
                            countryKey = country != null ? (Guid?)country.HeaderKey : null;
                        }
                            
                        continue;
                    }
                    
                    if (row.Attributes["class"].Value != "match" || competition == null)
                        continue;

                    if (!isCompetitionValid)
                        continue;

                    //Match
                    var matchId = row.Attributes["id"].Value.GetTextAfter("tgc");

                    //Team1
                    var teamRow = row.Descendants("td").First(r => r.Attributes.Contains("class") && r.Attributes["class"].Value == "team homeTeam");
                    var teamLink = teamRow.Descendants("a").First().Attributes["href"].Value;
                    var teamLookupId = teamLink.GetTextAfter("team.sd?team_id=");
                    var team1Key = await LookupTeam(teamLookupId, countryKey);

                    //Team2
                    teamRow = row.Descendants("td").First(r => r.Attributes.Contains("class") && r.Attributes["class"].Value == "team awayTeam");
                    teamLink = teamRow.Descendants("a").First().Attributes["href"].Value;
                    teamLookupId = teamLink.GetTextAfter("team.sd?team_id=");
                    var team2Key = await LookupTeam(teamLookupId, countryKey);

                    if (team1Key == null || team2Key == null)
                        continue;

                    await ImportMatch(competition, team1Key, team2Key, matchId, importDate);
                }
            }
        }

        private async Task ImportMatch(Competition competition, Guid? team1Key, Guid? team2Key, string matchId, DateTime matchDate)
        {
            // http://www.soccerbase.com/matches/additional_information.sd?id_game=689457
            var uri = new Uri(string.Format("http://www.soccerbase.com/matches/additional_information.sd?id_game={0}", matchId));
            var document = GetHtmlDocument(uri);
            
            //win - on aggregate
            var matchStatus = document.DocumentNode.Descendants("p").First(p => p.Attributes["class"].Value == "status").InnerText;
            var isCompleted = matchStatus == "result" || matchStatus.Contains("on aggregate") || matchStatus.Contains("on penalties") || matchStatus.Contains("AFTER EXTRA TIME") || matchStatus.Contains("win on away goals");

            if (!isCompleted)
                return;

            //Scores
            var span = document.DocumentNode.Descendants("span").FirstOrDefault(s => s.Attributes.Contains("title") && s.Attributes["title"].Value.Contains("Half-time"));
            var team1Ht = span != null ? span.Descendants("em").ElementAt(0).InnerText.ToNullableShort() : null;
            var team2Ht = span != null ? span.Descendants("em").ElementAt(1).InnerText.ToNullableShort() : null;

            span = document.DocumentNode.Descendants("span").FirstOrDefault(s => s.Attributes.Contains("title") && s.Attributes["title"].Value.Contains("Full-time"));
            var team1Ft = span != null ? span.Descendants("em").ElementAt(0).InnerText.ToNullableShort() : null;
            var team2Ft = span != null ? span.Descendants("em").ElementAt(1).InnerText.ToNullableShort() : null;

            //Attendance
            var attendance = GetAttendance(matchId);

            //Does match exist in Soccerbase lookup?
            var matchKey = await LookupMatch(matchId);
            Match match = null;
            MatchV matchV = null;

            //Does campaign exist?
            var campaign = await ProcessCampaign(competition.PrimaryKey, matchDate);

            if (campaign == null)
                return;

            var venueGuid = await GetVenueGuid((Guid)team1Key, matchDate);

            if (matchKey == null)
            {
                var endOfMatchDay = matchDate.ToEndOfDay();
                var beginningOfMatchDay = matchDate.Date;

                //Does match exist in Match tables
                //var matchQuery = from m in provider.MatchVs
                //                 where m.CampaignGuid == campaign.PrimaryKey
                //                 && m.Team1Guid == team1Key
                //                 && m.Team2Guid == team2Key
                //                 && m.IsActive
                //                 && !m.IsMarkedForDeletion
                //                 && m.MatchDate >= beginningOfMatchDay
                //                 && m.MatchDate <= endOfMatchDay
                //                 select m;

                var searchMatchV = await Provider.GetMatchByCampaignAndTeams(campaign.PrimaryKey, (Guid)team1Key, (Guid)team2Key, matchDate);

                if (searchMatchV != null)
                {
                    match = searchMatchV.Match;
                    matchKey = match.PrimaryKey;
                    matchV = match.GetApprovedVersion<MatchV>(matchDate);
                }
                else
                {
                    match = Match.Create<Match>();
                    matchV = MatchV.CreateNew<MatchV>(User.GetUserId());
                    matchV.HeaderKey = match.PrimaryKey;
                    matchV.EffectiveFrom = Date.LowDate;
                    matchV.EffectiveTo = Date.HighDate;

                    CreateLookupMatch(match.PrimaryKey, matchId);
                }
            }
            else
            {
                match = (await Provider.GetMatch((Guid)matchKey, matchDate)).Match;
                matchV = match.GetApprovedVersion<MatchV>(matchDate);
            }

            if (matchV.MatchImportType == MatchImportType.AutomaticResultWithEvents || matchV.MatchImportType == MatchImportType.ManualResult)
                return;
            
            matchV.MatchDate = matchDate.Date;
            matchV.MatchTimeTicks = matchV.MatchTimeTicks > 0 ? matchV.MatchTimeTicks : (matchDate - matchDate.Date).Ticks;
            matchV.MatchImportType = MatchImportType.AutomaticResultWithEvents;
            matchV.VenueGuid = venueGuid;
            matchV.Attendance = attendance;

            if (competition.GetApprovedVersion<CompetitionV>(matchDate).CompetitionType == CompetitionType.League)
            {
                var campaignStage = campaign.CampaignStages.FirstOrDefault(f => f.IsDefault);

                if (campaignStage != null)
                    matchV.CampaignStageKey = campaignStage.PrimaryKey;
            }

            matchV.Team1Guid = (Guid)team1Key;
            matchV.Team1HT = team1Ht;
            matchV.Team1FT = team1Ft;
            matchV.Team2Guid = (Guid)team2Key;
            matchV.Team2HT = team2Ht;
            matchV.Team2FT = team2Ft;

            if (matchKey == null)
            {
                Provider.Add(match);
                Provider.Add(matchV);
            }

            Provider.SaveChanges();

            await ProcessMatchPlayers(document, matchV);
        }

        private int? GetAttendance(string matchId)
        {
            // http://www.soccerbase.com/teams/lineup_bubble.sd?id_game=689951
            var uri = new Uri(string.Format("http://www.soccerbase.com/teams/lineup_bubble.sd?id_game={0}", matchId));
            var document = GetHtmlDocument(uri);

            ImportType importType = ImportType.Default;

            var list = document.DocumentNode.Descendants("dl").FirstOrDefault();

            if (list == null)
                return null;

            foreach (var item in list.Descendants().Where(d => d.Name == "dd" || d.Name == "dt"))
            {
                switch (item.InnerText)
                {
                    case "Referee:":
                        importType = ImportType.Referee;
                        break;

                    case "Attendance:":
                        importType = ImportType.Attendance;
                        break;

                    case "Stadium:":
                        importType = ImportType.Venue;
                        break;

                    default:
                        if (importType == ImportType.Attendance)
                        {
                            return item.InnerText.ToNullableInt();
                        }
                        importType = ImportType.Default;
                        break;
                }
            }

            return null;
        }

        private async Task ProcessMatchPlayers(HtmlDocument document, MatchV matchV)
        {
            var playersDiv = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "matchPanel");

            if (playersDiv == null)
                return;

            var team1Div = playersDiv.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "soccerColumn teamA");

            if (team1Div != null)
            {
                await ImportPlayers(matchV, true, team1Div, MatchEventType.Started);
                await ImportPlayers(matchV, true, team1Div, MatchEventType.Substitute);
            }

            var team2Div = playersDiv.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "soccerColumn teamB");

            if (team2Div != null)
            {
                await ImportPlayers(matchV, false, team2Div, MatchEventType.Started);
                await ImportPlayers(matchV, false, team2Div, MatchEventType.Substitute);
            }

            await ImportGoalscorers(matchV, document, true);
            await ImportGoalscorers(matchV, document, false);

            Provider.SaveChanges();
        }

        private async Task ImportPlayers(MatchV matchV, bool isTeam1, HtmlNode teamDiv, MatchEventType matchEventType)
        {
            var startersTbody = matchEventType == MatchEventType.Started
                ? teamDiv.Descendants("tbody").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "firstTeam")
                : teamDiv.Descendants("tbody").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "reserve");

            if (startersTbody != null)
            {
                foreach (var tr in startersTbody.Descendants("tr"))
                {
                    short? sentOffTime = null;
                    var teamGuid = isTeam1 ? matchV.Team1Guid : matchV.Team2Guid;

                    //Starters-Subs
                    var link = tr.Descendants("a").FirstOrDefault(t => t.Attributes.Contains("href"));

                    if (link == null)
                        break;

                    var lookupId = link.Attributes["href"].Value.GetTextAfter("player.sd?player_id=");

                    var positionString = tr.Descendants("td").ElementAt(isTeam1 ? 1 : 2).InnerText.GetTextBetween("(", ")");

                    var personKey = await ProcessPlayer(lookupId);

                    if (personKey == null)
                        continue;

                    await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, GetPositionType(positionString), matchEventType, null, null);

                    //Substitutions
                    var timeText = tr.Descendants("td").ElementAt(isTeam1 ? 0 : 3).InnerText.GetTextBetween("(",")").Trim();

                    if (!string.IsNullOrWhiteSpace(timeText))
                    {
                        if (matchEventType == MatchEventType.Started)
                        {
                            // (s/o 90)
                            if (timeText.IndexOf("s/o") >= 0)
                                sentOffTime = timeText.GetTextAfter("s/o").Trim().ToNullableShort();
                            else
                            {
                                var substitutedTime = timeText.ToNullableShort();

                                if (substitutedTime != null)
                                    await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.TakenOff, substitutedTime, null);
                            }
                        }
                        
                        if (matchEventType == MatchEventType.Substitute)
                        {
                            // (85, s/o 85) brought on and sent off
                            // (77-89) brought on and substituted
                            if (timeText.IndexOf(",") >= 0)
                            {
                                var broughtOnTime = timeText.GetTextBefore(",").Trim().ToNullableShort();
                                
                                if (broughtOnTime != null)
                                    await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.BroughtOn, broughtOnTime, null);

                                var remainingText = timeText.GetTextAfter(",");

                                if (remainingText.IndexOf("s/o") >= 0)
                                    sentOffTime = remainingText.GetTextAfter("s/o").Trim().ToNullableShort();
                            }
                            else if (timeText.IndexOf("-") >= 0)
                            {
                                var broughtOnTime = timeText.GetTextBefore("-").Trim().ToNullableShort();

                                if (broughtOnTime != null)
                                    await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.BroughtOn, broughtOnTime, null);

                                var remainingText = timeText.GetTextAfter("-").Trim();

                                var takenOffTime = remainingText.Trim().ToNullableShort();

                                if (takenOffTime != null)
                                    await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.TakenOff, takenOffTime, null);
                            }
                            else
                            {
                                var broughtOnTime = timeText.Trim().ToNullableShort();

                                if (broughtOnTime != null)
                                    await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.BroughtOn, broughtOnTime, null);
                            }

                        }
                    }

                    //Discipline
                    var image = tr.Descendants("td").ElementAt(isTeam1 ? 3 : 0).Descendants("img").FirstOrDefault();

                    if (image != null)
                    {
                        switch (image.Attributes["title"].Value)
                        {
                            case "Yellow card":
                                await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.Booked, null, null);
                                break;

                            case "Red card":
                                await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.SentOff, sentOffTime, null);
                                break;


                            case "Red and yellow cards":
                                await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.Booked, null, null);
                                await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, MatchEventType.SentOff, sentOffTime, null);
                                break;

                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        private async Task ImportGoalscorers(MatchV matchV, HtmlDocument document, bool isTeam1)
        {
            var teamGuid = isTeam1 ? matchV.Team1Guid : matchV.Team2Guid;
            var goalscorersDiv = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "goalscorers");

            if (goalscorersDiv == null)
                return;

            var teamDiv = isTeam1
                ? goalscorersDiv.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "teamA")
                : goalscorersDiv.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "teamB");

            if (teamDiv != null)
            {
                foreach (var scorerSpan in teamDiv.Descendants("span"))
                {
                    var playerLink = scorerSpan.Descendants("a").FirstOrDefault(a => a.Attributes.Contains("href"));

                    if (playerLink == null || !playerLink.Attributes.Contains("href"))
                        continue;

                    var playerHref = playerLink.Attributes["href"].Value;
                    var lookupId = playerHref.GetTextAfter("player.sd?player_id=");

                    var personKey = await ProcessPlayer(lookupId);

                    if (personKey == null)
                        continue;

                    var scoreTimes = scorerSpan.InnerText.GetTextBetween("(", ")").Trim().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var scoreTimeText in scoreTimes)
                    {
                        if (scoreTimeText.IndexOf("s/o") >= 0)
                            continue;

                        var matchEventType = scoreTimeText.IndexOf("og") >= 0 ? MatchEventType.OwnGoal : MatchEventType.Scored;

                        var scoreTime = scoreTimeText.Replace("pen", "").Replace("og", "").Trim().ToNullableShort();

                        if (matchEventType != MatchEventType.OwnGoal)
                            await ProcessMatchEvent(matchV.PrimaryKey, teamGuid, (Guid)personKey, null, matchEventType, scoreTime, null);
                        else
                            await ProcessMatchEvent(matchV.PrimaryKey, isTeam1 ? matchV.Team2Guid : matchV.Team1Guid, (Guid)personKey, null, matchEventType, scoreTime, null);
                    }
                }
            }
        }

        private async Task<Guid?> LookupTeam(string lookupId, Guid? countryKey)
        {
            var lookupTeam = await Provider.GetLookupTeam(ImportSite, lookupId);

            if (lookupTeam != null)
                return (Guid?)lookupTeam.TeamGuid;

            return AddNewTeam(lookupId, countryKey);
        }

        private async Task<Guid?> ProcessPlayer(string lookupId)
        {
            var personGuid = await LookupPerson(lookupId);

            if (personGuid != null)
                return personGuid;

            // http://www.soccerbase.com/players/player.sd?player_id=46629
            var uri = new Uri(string.Format("http://www.soccerbase.com/players/player.sd?player_id={0}", lookupId));
            var document = GetHtmlDocument(uri);

            var playerDiv = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "soccerContent");

            if (playerDiv == null)
                return null;

            string forenames = string.Empty;
            string surname = string.Empty;
            DateTime? dob = null;
            double? height = null;
            Guid? countryKey = null;

            foreach (var tr in playerDiv.Descendants("tr"))
            {
                var th = tr.Descendants("th").FirstOrDefault();

                if (th == null)
                    continue;

                switch (th.InnerText)
                {
                    case "Real name":
                        var fullName = tr.Descendants("strong").First().InnerText;
                        ResolvePersonNames(fullName, out forenames, out surname);

                        if (string.IsNullOrWhiteSpace(forenames) && string.IsNullOrWhiteSpace(surname))
                            return null;

                        break;

                    case "Age":
                        var dobString = tr.Descendants("td").First().InnerText.GetTextBetween("(Born ", ")");
                        dob = !string.IsNullOrWhiteSpace(dobString) ? (DateTime?)DateTime.Parse(dobString) : null;
                        break;

                    case "Height":
                        var heightString = tr.Descendants("strong").First().InnerText.GetTextBetween("(", "m)");

                        if (!string.IsNullOrWhiteSpace(heightString))
                            height = Convert.ToDouble(heightString) * 100;
                        break;

                    case "Nationality":
                        var nationalityString = tr.Descendants("strong").First().InnerText.Trim();
                        countryKey = await GetCountryKeyByName(nationalityString);
                        break;
                }
            }

            var person = Person.Create<Person>();
            person.PrimaryKey = Guid.NewGuid();

            var personV = PersonV.CreateNew<PersonV>(User.GetUserId());
            personV.HeaderKey = person.PrimaryKey;
            personV.EffectiveFrom = Date.LowDate;
            personV.EffectiveTo = Date.HighDate;
            personV.Forenames = forenames;
            personV.Surname = surname;
            personV.SearchText = personV.SetSearchText();
            personV.CountryGuid = countryKey;
            personV.DateOfBirth = dob;
            personV.Height = Convert.ToInt32(height);

            Provider.Add(person);
            Provider.Add(personV);
            
            Provider.Add(new LookupPerson()
            {
                PrimaryKey = Guid.NewGuid(),
                ImportSite = this.ImportSite,
                LookupId = lookupId,
                PersonGuid = person.PrimaryKey
            });

            Provider.SaveChanges();

            return person.PrimaryKey;
        }

        private Guid? AddNewTeam(string teamLookupId, Guid? countryKey)
        {
            // http://www.soccerbase.com/teams/team.sd?team_id=485
            var uri = new Uri(string.Format("http://www.soccerbase.com/teams/team.sd?team_id={0}", teamLookupId));
            var document = GetHtmlDocument(uri);

            var table = document.DocumentNode.Descendants("table").FirstOrDefault(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "imageHead");
            var header1 = table.Descendants("h1").FirstOrDefault();
            var teamName = header1.InnerText.GetTextBefore("Club details").Replace("/n", string.Empty).Trim();

            var venueName = string.Empty;
            int? venueCapacity = null;

            table = document.DocumentNode.Descendants("table").FirstOrDefault(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "clubInfo");

            foreach (var tr in table.Descendants("tr"))
            {
                var th = tr.Descendants("th").FirstOrDefault();

                if (th == null)
                    continue;

                switch (th.InnerText)
                {
                    case "Ground":
                        var venueNode = tr.Descendants("strong").FirstOrDefault();

                        if (venueNode != null)
                            venueName = venueNode.InnerText.Trim();

                        var groundList = tr.Descendants("li").FirstOrDefault(l => l.Attributes.Contains("class") && l.Attributes["class"].Value == "alt");

                        if (groundList != null)
                        {
                            var capacityNode = groundList.Descendants("strong").FirstOrDefault();

                            if (capacityNode != null)
                                venueCapacity = capacityNode.InnerText.Replace("/n", string.Empty).Replace(",", string.Empty).Trim().ToNullableInt();
                        }

                        break;
                }
            }

            Guid? venueKey = null;

            if (!string.IsNullOrWhiteSpace(venueName) && countryKey != null)
            {
                venueKey = Guid.NewGuid();
                var venue = new Venue() {PrimaryKey = (Guid)venueKey};
                Provider.Add(venue);

                var venueV = VenueV.CreateNew<VenueV>(User.GetUserId());
                venueV.HeaderKey = venue.PrimaryKey;
                venueV.VenueName = venueName;
                venueV.CountryGuid = (Guid)countryKey;
                venueV.Capacity = venueCapacity;
                venueV.EffectiveFrom = Date.LowDate;
                venueV.EffectiveTo = Date.HighDate;

                Provider.Add(venueV);
            }

            Guid? teamGuid = null;

            if (!string.IsNullOrEmpty(teamName))
            {
                teamGuid = Guid.NewGuid();
                var team = new Team() {PrimaryKey = (Guid)teamGuid};
                Provider.Add(team);

                var teamV = TeamV.CreateNew<TeamV>(User.GetUserId());
                teamV.HeaderKey = team.PrimaryKey;
                teamV.HomeVenueGuid = venueKey;
                teamV.CountryGuid = countryKey;
                teamV.EffectiveFrom = Date.LowDate;
                teamV.EffectiveTo = Date.HighDate;

                teamV.TeamNames = new List<TeamName>();

                teamV.TeamNames.Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = teamV.PrimaryKey,
                    TeamNameType = TeamNameType.Primary,
                    LanguageType = LanguageType.Native,
                    Description = teamName
                });

                Provider.Add(teamV);

                Provider.Add(new LookupTeam()
                {
                    PrimaryKey = Guid.NewGuid(),
                    ImportSite = this.ImportSite,
                    TeamGuid = (Guid)teamGuid,
                    LookupId = teamLookupId
                });
            }

            Provider.SaveChanges();

            return teamGuid; 
        }
    }
}
