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

namespace FootballOracle.Services.Import
{
    public class Espn : BaseImport
    {
        public Espn(IRepositoryProvider provider, IPrincipal user) 
            : base(provider, ImportSite.Espn, user)
        {
        }

        private Guid? countryGuid;

        public async Task Import(CompetitionV competitionV, bool includeFixtures, bool includeResults, DateTime viewDate)
        {
            var startDate = Date.LowDate;
            var endDate = Date.HighDate;

            if (!competitionV.GetCampaignDates(viewDate, ref startDate, ref endDate))
                return;

            // http://www.espn.co.uk/football/sport/match/index.html?event=3;page=5;season=2013%2F14
            // http://www.espn.co.uk/football/sport/match/index.html?event=3;type=fixtures

            var country = competitionV.Competition.GetParentCountry(viewDate);

            countryGuid = country != null ? (Guid?)country.HeaderKey : null;

            var seasonId = string.Format("{0}%2F{1}", startDate.ToString("yyyy"), endDate.ToString("yy"));
            var eventId = (await Provider.GetLookupCompetition(competitionV.HeaderKey, ImportSite)).LookupId;

            if (includeFixtures)
            {
                var uri = new Uri(string.Format("http://www.espn.co.uk/football/sport/match/index.html?event={0};type=fixtures", eventId));
                await ImportMatches(competitionV, uri, eventId);
            }

            var resultsFound = true;
            var page = 1;

            if (includeResults)
            {
                while (resultsFound)
                {
                    var uri = new Uri(string.Format("http://www.espn.co.uk/football/sport/match/index.html?event={0};page={1};season={2}", eventId, page, seasonId));
                    resultsFound = await ImportMatches(competitionV, uri, eventId);
                    page++;
                }
            }
        }

        private async Task<bool> ImportMatches(CompetitionV competitionV, Uri uri, string eventId)
        {
            var resultsFound = false;

            var document = GetHtmlDocument(uri);
            var table = document.DocumentNode.Descendants("table").Where(tb => tb.Attributes["class"].Value == "engineTable").FirstOrDefault();

            var baseDate = Date.LowDate;

            foreach (var tr in table.Descendants("tr"))
            {
                if (tr.Descendants("td").Any(t => t.InnerText == "No matches found"))
                    return false;

                switch (tr.Attributes["class"].Value)
                {
                    case "heading":
                        var temp = tr.Descendants("a").First().Attributes["href"].Value;
                        baseDate = DateTime.Parse(temp.GetTextBetween("?date=", ";"));
                        break;

                    case "data1":
                        var columnCount = tr.Descendants("td").Count();

                        //Skip check
                        if (eventId == "21" && tr.Descendants("td").Count() >= 7 && tr.Descendants("td").ElementAt(6).InnerText.IndexOf("HASH") > 0)
                            break;

                        if (tr.InnerText.Contains("After extra time") || tr.InnerText.Contains("penalties in progress"))
                            break;

                        var matchDate = GetMatchTime(tr, baseDate);
                        var team1Guid = await GetTeamGuid(tr, true);
                        var team2Guid = await GetTeamGuid(tr, false);
                        var venueGuid = await GetVenueGuid(team1Guid, matchDate);
                        var team1Ft = GetFullTimeScore(tr, true);
                        var team2Ft = GetFullTimeScore(tr, false);
                        var team1Ht = GetHalfTimeScore(tr, true);
                        var team2Ht = GetHalfTimeScore(tr, false);
                        var attendance = GetAttendance(tr);
                        var matchId = GetMatchLookupId(tr);
                        var campaignStageKey = GetCampaignStage(tr, competitionV, matchDate);

                        //MatchType matchType = MatchType.League;
                        //StageType? stageType = null;

                        //if (competitionV.CompetitionType == CompetitionType.Cup)
                        //    GetMatchAndStageTypes(tr, out matchType, out stageType);

                        await ProcessMatch(matchId, matchDate, competitionV, venueGuid, attendance, team1Guid, team1Ht, team1Ft, team2Guid, team2Ht, team2Ft, campaignStageKey);
                        resultsFound = true;
                        break;
                }
            }

            return resultsFound;
        }

        private DateTime GetMatchTime(HtmlNode tr, DateTime baseDate)
        {
            var columnCount = tr.Descendants("td").Count();

            if (columnCount <= 3)
                return baseDate;

            var temp = tr.Descendants("td").ElementAt(0).InnerText.Trim();

            if (string.IsNullOrEmpty(temp))
                return baseDate;

            return baseDate.Add(TimeSpan.Parse(temp));
        }

        private async Task<Guid> GetTeamGuid(HtmlNode tr, bool isTeam1)
        {
            var links = tr.Descendants("a").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value == "data-link");

            var link = isTeam1 ? links.ElementAt(0) : links.ElementAt(1);
            var temp = link.Attributes["href"].Value;
            var teamLookupId = temp.GetTextAfter(";team=").Trim();
            var teamName = link.InnerText.Trim();

            return await ProcessTeam(teamLookupId, teamName, countryGuid);
        }

        private short? GetFullTimeScore(HtmlNode tr, bool isTeam1)
        {
            var cells = tr.Descendants("td").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "centre espn_nowrap");

            if (!cells.Any())
                return null;

            var temp = cells.ElementAt(0).InnerText.Trim();

            return isTeam1 ? temp.GetTextBefore("-").ToNullableShort() : temp.GetTextAfter("-").ToNullableShort();
        }

        private short? GetHalfTimeScore(HtmlNode tr, bool isTeam1)
        {
            var cells = tr.Descendants("td").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "centre espn_nowrap");

            if (cells.Count() < 2)
                return null;

            var temp = cells.ElementAt(1).InnerText.Trim();

            return isTeam1 ? temp.GetTextBefore("-").ToNullableShort() : temp.GetTextAfter("-").ToNullableShort();
        }

        private int? GetAttendance(HtmlNode tr)
        {
            var columnCount = tr.Descendants("td").Count();

            if (columnCount < 6)
                return null;

            var temp = tr.Descendants("td").ElementAt(columnCount - 2).InnerText.Trim().Replace(",", string.Empty);
            return temp.ToNullableInt();
        }

        private string GetMatchLookupId(HtmlNode tr)
        {
            var link = tr.Descendants("td").Last().Descendants("a").LastOrDefault();
            var temp = link != null ? link.Attributes["href"].Value : string.Empty;
            return temp.GetTextBetween("match/", ".html");
        }

        private Guid GetCampaignStage(HtmlNode tr, CompetitionV competitionV, DateTime matchDate)
        {
            var campaign = competitionV.Competition.Campaigns.SingleOrDefault(s => s.StartDate <= matchDate && s.EndDate >= matchDate);

            var cells = tr.Descendants("td").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "left");

            foreach (var cell in cells)
            {
                foreach (var campaignStage in campaign.CampaignStages)
                {
                    var lookup = campaignStage.LookupCampaignStages.FirstOrDefault(w => w.ImportSite == ImportSite && cell.InnerText.Trim() == w.LookupId);

                    if (lookup != null)
                        return lookup.CampaignStageKey;
                }
            }

            var defaultStage = campaign.CampaignStages.SingleOrDefault(s => s.IsDefault);

            return defaultStage.PrimaryKey;
        }

        public async Task ImportMatchDetail(Guid matchId)
        {
            // http://www.espnfc.co.uk/gamecast/statistics/id/368752/statistics.html

            var matchV = (await Provider.GetMatch(matchId, DateTime.Now));

            var lookupMatch = (await Provider.GetLookupMatch(matchV.HeaderKey)).SingleOrDefault(w => w.ImportSite == ImportSite);

            if (lookupMatch == null || lookupMatch.HasMatchDetails)
                return;

            var uri = new Uri(string.Format("http://www.espnfc.co.uk/gamecast/statistics/id/{0}/statistics.html", lookupMatch.LookupId));
            var document = GetHtmlDocument(uri);

            var locationString = document.DocumentNode.SelectNodes(@"//div[@class=""match-details""]/p")[1].InnerText.Trim();
            locationString = locationString.GetTextBefore(",");

            var defaultCountryGuid = matchV.CampaignStage.Campaign.Competition.GetParentCountry(matchV.MatchDate).HeaderKey;

            var venueGuid = await ProcessVenue(locationString, defaultCountryGuid); 

            var mainDiv = document.DocumentNode.SelectNodes(@"//div[@class=""matchup""]").FirstOrDefault();

            if (mainDiv == null)
                return;

            var teamGuid = matchV.Team1Guid;

            foreach (var statTable in mainDiv.SelectNodes("//table[@class=\"stat-table\"]"))
            {
                var isSubstitute = false;

                foreach (var row in statTable.SelectNodes("tbody/tr"))
                {
                    if (row.SelectSingleNode("th") != null)
                    {
                        if (row.SelectSingleNode("th").Attributes["class"].Value == "tb-substitutes home-team" || row.SelectSingleNode("th").Attributes["class"].Value == "tb-substitutes away-team")
                            isSubstitute = true;
                    }

                    if (row.SelectSingleNode("td") == null || row.SelectNodes("td").Count < 2)
                        continue;

                    var position = row.SelectNodes("td")[0].InnerText;
                    var fullName = row.SelectNodes("td")[2].SelectSingleNode("a").InnerText.Trim();

                    string forenames;
                    string surname;
                    ResolvePersonNames(fullName, out forenames, out surname);

                    var working = row.SelectNodes("td")[2].SelectSingleNode("a").Attributes["href"].Value;
                    var espnPersonId = working.GetTextBetween("player/_/id/", "/");

                    var personGuid = await ProcessPerson(espnPersonId, forenames, surname, null, null, null);

                    var appearanceAttributes = row.SelectNodes("td")[2].SelectNodes("div");
                    var startingStatus = isSubstitute ? MatchEventType.Substitute : MatchEventType.Started;

                    Provider.Add(new MatchEvent() 
                    {
                        PrimaryKey = Guid.NewGuid(),
                        MatchVPrimaryKey = matchV.PrimaryKey,
                        PersonPrimaryKey = personGuid,
                        PositionType = GetPositionType(position),
                        TeamPrimaryKey = teamGuid,
                        MatchEventType = startingStatus,
                        Minute = null,
                        Extra = null
                    });

                    Provider.SaveChanges();

                    if (appearanceAttributes != null)
                    {
                        foreach (var appearanceAttribute in appearanceAttributes)
                        {
                            short? minute = null;
                            short? plus = null;
                            var matchEventType = GetMatchEvent(appearanceAttribute, out minute, out plus);

                            if (matchEventType != null)
                            {
                                Provider.Add(new MatchEvent()
                                {
                                    PrimaryKey = Guid.NewGuid(),
                                    MatchVPrimaryKey = matchV.PrimaryKey,
                                    PersonPrimaryKey = personGuid,
                                    PositionType = null,
                                    TeamPrimaryKey = teamGuid,
                                    MatchEventType = (MatchEventType)matchEventType,
                                    Minute = minute,
                                    Extra = plus
                                });

                                Provider.SaveChanges();
                            }

                            if (minute <= 45)
                            {
                                if (matchEventType == MatchEventType.Scored && teamGuid == matchV.Team1Guid || matchEventType == MatchEventType.OwnGoal && teamGuid == matchV.Team2Guid)
                                    matchV.Team1HT++;

                                if (matchEventType == MatchEventType.Scored && teamGuid == matchV.Team2Guid || matchEventType == MatchEventType.OwnGoal && teamGuid == matchV.Team1Guid)
                                    matchV.Team2HT++;
                            }
                        }
                    }
                }

                teamGuid = matchV.Team2Guid;

                await ProcessMatch(lookupMatch.LookupId, matchV.MatchDate, matchV.CampaignStage.Campaign.Competition.GetApprovedVersion<CompetitionV>(matchV.MatchDate), venueGuid, matchV.Attendance, matchV.Team1Guid, matchV.Team1HT, matchV.Team1FT, matchV.Team2Guid, matchV.Team2HT, matchV.Team2FT, matchV.CampaignStageKey);
                lookupMatch.HasMatchDetails = true;
                Provider.SaveChanges();
            }
        }

        private static MatchEventType? GetMatchEvent(HtmlNode appearanceAttribute, out short? minute, out short? plus)
        {
            minute = null;
            plus = null;
            string working = appearanceAttribute.Attributes["onmouseover"].Value.GetTextBetween("</strong> - ", "\\'").Trim();
            int tryNumber;

            if (int.TryParse(working, out tryNumber))
                minute = (short)tryNumber;
            else
            {
                int plusPosition = working.IndexOf("+");

                if (plusPosition > 0)
                {
                    string lhs = working.Substring(0, plusPosition).Trim();

                    if (int.TryParse(lhs, out tryNumber))
                        minute = (short)tryNumber;

                    string rhs = working.Substring(plusPosition + 1);

                    if (int.TryParse(rhs, out tryNumber))
                        plus = (short)tryNumber;
                }
            }

            switch (appearanceAttribute.Attributes["class"].Value)
            {
                case "soccer-icons soccer-icons-redcard":
                    return MatchEventType.SentOff;

                case "soccer-icons soccer-icons-yellowcard":
                    return MatchEventType.Booked;

                case "soccer-icons soccer-icons-goal":
                    return MatchEventType.Scored;

                case "soccer-icons soccer-icons-owngoal":
                    return MatchEventType.OwnGoal;

                case "soccer-icons soccer-icons-subinout":
                    return MatchEventType.BroughtOn;

                default:
                    return null;
            }
        }
    }
}
