using FootballOracle.Foundation;
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
    public abstract class BaseImport
    {
        public BaseImport(IRepositoryProvider provider,ImportSite importSite, IPrincipal user)
        {
            this.Provider = provider;
            this.ImportSite = importSite;
            this.User = user;
        }

        protected ImportSite ImportSite;
        protected IPrincipal User;
        protected IRepositoryProvider Provider;

        protected static HtmlDocument GetHtmlDocument(Uri uri)
        {
            HtmlWeb web = new HtmlWeb();
            web.UsingCache = false;

            try
            {
                return web.Load(uri.AbsoluteUri);
            }
            catch
            {
                return new HtmlDocument();
            }
        }

        protected static void ResolvePersonNames(string fullName, out string forenames, out string surname)
        {
            forenames = string.Empty;
            surname = string.Empty;

            if (fullName.IndexOf(" ") > 0)
            {
                for (int pos = fullName.Length - 1; pos > 0; pos--)
                {
                    if (fullName.Substring(pos, 1) == " ")
                    {
                        forenames = fullName.Substring(0, pos).Trim();
                        surname = fullName.Substring(pos).Trim();
                        break;
                    }
                }
            }
            else
                surname = fullName;
        }

        protected static PositionType? GetPositionType(string value)
        {
            switch (value)
            {
                case "G":
                    return PositionType.Goalkeeper;

                case "D":
                    return PositionType.Defender;

                case "M":
                    return PositionType.Midfielder;

                case "F":
                    return PositionType.Forward;
            }

            return null;
        }

        public string LogFilePath { get; set; }

        protected async Task<Competition> LookupCompetition(string lookupId)
        {
            var lookupCompetition = await Provider.GetLookupCompetition(ImportSite, lookupId);

            return lookupCompetition != null ? lookupCompetition.Competition : null;
        }

        protected async Task<Guid?> LookupMatch(string lookupId)
        {
            var lookupMatch = await Provider.GetLookupMatch(ImportSite, lookupId);

            return lookupMatch != null ? (Guid?)lookupMatch.MatchGuid : null;
        }

        protected async Task<Campaign> ProcessCampaign(Guid competitionKey, DateTime matchDate)
        {
            var campaign = await Provider.FindCampaignAsync(competitionKey, matchDate);

            if (campaign != null)
                return campaign;

            var competitionViewModel = await Provider.GetCompetition(competitionKey, matchDate);

            if (competitionViewModel == null)
                return null;

            var startDate = Date.LowDate;
            var endDate = Date.HighDate;

            if (!competitionViewModel.GetCampaignDates(matchDate, ref startDate, ref endDate))
                return null;

            var newCampaign = Campaign.CreateNew(competitionViewModel.HeaderKey, startDate, endDate);
            campaign = newCampaign;

            Provider.Add(newCampaign);
            Provider.SaveChanges();

            return campaign;
        }

        protected async Task ProcessMatch(string lookupId, DateTime matchDate, CompetitionV competitionV, Guid? venueGuid, int? attendance, Guid team1Guid, short? team1Ht, short? team1Ft, Guid team2Guid, short? team2Ht, short? team2Ft, Guid campaignStageKey)
        {
            if (campaignStageKey == null)
                return;

            var lookupMatchSearch = await Provider.GetLookupMatch(ImportSite, lookupId);

            var startOfDay = matchDate.Date;
            var endOfDay = matchDate.ToEndOfDay();

            var matchSearch = await Provider.GetMatchByTeams(team1Guid, team2Guid, matchDate);

            var campaign = await Provider.FindCampaignAsync(competitionV.HeaderKey, matchDate);

            if (campaign == null)
            {
                var startDate = Date.LowDate;
                var endDate = Date.HighDate;

                if (!competitionV.GetCampaignDates(matchDate, ref startDate, ref endDate))
                    return;

                var newCampaign = Campaign.CreateNew(competitionV.HeaderKey, startDate, endDate);
                campaign = newCampaign;

                Provider.Add(newCampaign);
                Provider.SaveChanges();
            }

            if (lookupMatchSearch != null || matchSearch != null)
            {
                MatchV matchV = null;

                if (lookupMatchSearch != null)
                {
                    var lookupMatch = lookupMatchSearch;
                    matchV = (await Provider.GetMatch(lookupMatch.MatchGuid, DateTime.Now));
                }
                else if (matchSearch != null)
                    matchV = matchSearch;

                if (matchV == null || matchV.MatchImportType == MatchImportType.ManualResult)
                    return;

                matchV.MatchDate = matchDate.Date;
                matchV.MatchTimeTicks = (matchDate - matchDate.Date).Ticks;
                matchV.VenueGuid = venueGuid;
                matchV.Attendance = attendance;
                matchV.Team1Guid = team1Guid;
                matchV.Team1HT = team1Ht;
                matchV.Team1FT = team1Ft;
                matchV.Team2Guid = team2Guid;
                matchV.Team2HT = team2Ht;
                matchV.Team2FT = team2Ft;
                matchV.MatchImportType = matchV.GetMatchImportType(true);
                matchV.CampaignStageKey = campaignStageKey;
            }
            else
            {
                if (matchDate < DateTime.Now && team1Ft == null && team2Ft == null)
                    return;

                var matchGuid = Guid.NewGuid();

                Provider.Add(new Match() { PrimaryKey = matchGuid });

                var matchV = MatchV.CreateNew<MatchV>(User.GetUserId());
                matchV.HeaderKey = matchGuid;
                matchV.MatchDate = matchDate.Date;
                matchV.MatchTimeTicks = (matchDate - matchDate.Date).Ticks;
                matchV.VenueGuid = venueGuid;
                matchV.Attendance = attendance;
                matchV.Team1Guid = team1Guid;
                matchV.Team1HT = team1Ht;
                matchV.Team1FT = team1Ft;
                matchV.Team2Guid = team2Guid;
                matchV.Team2HT = team2Ht;
                matchV.Team2FT = team2Ft;
                matchV.EffectiveFrom = Date.LowDate;
                matchV.EffectiveTo = Date.HighDate;
                matchV.MatchImportType = matchV.GetMatchImportType(true);
                matchV.CampaignStageKey = campaignStageKey;

                Provider.Add(matchV);

                if (lookupId != string.Empty)
                {
                    Provider.Add(new LookupMatch()
                    {
                        PrimaryKey = Guid.NewGuid(),
                        ImportSite = ImportSite,
                        MatchGuid = matchGuid,
                        LookupId = lookupId
                    });
                }
            }

            Provider.SaveChanges();
        }

        protected async Task ProcessMatchEvent(Guid matchVKey, Guid teamKey, Guid personKey, PositionType? positionType, MatchEventType matchEventType, short? minute, short? extra)
        {
            var matchEventQuery = await Provider.GetMatchEvents(matchVKey, teamKey, personKey, matchEventType, minute, extra);

            if (matchEventQuery.Any())
                return;

            Provider.Add(new MatchEvent()
            {
                PrimaryKey = Guid.NewGuid(),
                MatchVPrimaryKey = matchVKey,
                TeamPrimaryKey = teamKey,
                PersonPrimaryKey = personKey,
                PositionType = positionType,
                MatchEventType = matchEventType,
                Minute = minute,
                Extra = extra
            });
        }

        protected async Task<Guid> ProcessPerson(string lookupId, string forenames, string surname, DateTime? dateOfBirth, DateTime? dateOfDeath, Guid? countryGuid)
        {
            var lookupPerson = await Provider.GetLookupPerson(ImportSite, lookupId);

            if (lookupPerson != null)
                return lookupPerson.PersonGuid;
            else
            {
                var personGuid = Guid.NewGuid();

                Provider.Add(new Person() { PrimaryKey = personGuid });

                var personV = PersonV.CreateNew<PersonV>(User.GetUserId());
                personV.HeaderKey = personGuid;
                personV.Forenames = forenames;
                personV.Surname = surname;
                personV.SearchText = personV.SetSearchText();
                personV.DateOfBirth = dateOfBirth;
                personV.DateOfDeath = dateOfDeath;
                personV.CountryGuid = countryGuid;
                personV.EffectiveFrom = Date.LowDate;
                personV.EffectiveTo = Date.HighDate;

                Provider.Add(personV);

                Provider.Add(new LookupPerson()
                {
                    PrimaryKey = Guid.NewGuid(),
                    ImportSite = ImportSite,
                    PersonGuid = personGuid,
                    LookupId = lookupId
                });

                Provider.SaveChanges();

                return personGuid;
            }
        }

        protected async Task<Guid> ProcessTeam(string lookupId, string teamName, Guid? countryGuid)
        {
            var lookupTeamSearch = await Provider.GetLookupTeam(ImportSite, lookupId);

            if (lookupTeamSearch != null)
                return lookupTeamSearch.TeamGuid;
            else
            {
                var teamGuid = Guid.NewGuid();

                Provider.Add(new Team() { PrimaryKey = teamGuid });

                var teamV = TeamV.CreateNew<TeamV>(User.GetUserId());
                teamV.HeaderKey = teamGuid;
                teamV.CountryGuid = countryGuid;
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
                    ImportSite = ImportSite,
                    TeamGuid = teamGuid,
                    LookupId = lookupId
                });

                Provider.SaveChanges();

                return teamGuid;
            }
        }

        protected async Task<Guid> ProcessVenue(string lookupId, Guid? countryGuid)
        {
            var lookupVenueSearch = await Provider.GetLookupVenue(ImportSite, lookupId);

            if (lookupVenueSearch != null)
                return lookupVenueSearch.VenueGuid;
            else
            {
                var venueGuid = Guid.NewGuid();

                Provider.Add(new Venue() { PrimaryKey = venueGuid });

                var venueV = VenueV.CreateNew<VenueV>(User.GetUserId());
                venueV.HeaderKey = venueGuid;
                venueV.VenueName = lookupId;
                venueV.CountryGuid = (Guid)countryGuid;
                venueV.EffectiveFrom = Date.LowDate;
                venueV.EffectiveTo = Date.HighDate;

                Provider.Add(venueV);

                Provider.Add(new LookupVenue()
                {
                    PrimaryKey = Guid.NewGuid(),
                    ImportSite = ImportSite,
                    VenueGuid = venueGuid,
                    LookupId = lookupId
                });

                Provider.SaveChanges();

                return venueGuid;
            }
        }

        protected void CreateLookupMatch(Guid matchKey, string lookupId)
        {
            Provider.Add(new LookupMatch()
            {
                PrimaryKey = Guid.NewGuid(),
                MatchGuid = matchKey,
                LookupId = lookupId,
                ImportSite = this.ImportSite,
                HasMatchDetails = true
            });
        }

        

        protected async Task<Guid?> LookupPerson(string lookupId)
        {
            var lookupPlayer = await Provider.GetLookupPerson(ImportSite, lookupId);

            return lookupPlayer != null ? (Guid?)lookupPlayer.PersonGuid : null;
        }

        protected async Task<Guid?> GetVenueGuid(Guid team1Key, DateTime matchDate)
        {
            var team = await Provider.GetTeam(team1Key, matchDate);

            return team != null ? team.HomeVenueGuid : null;
        }

        protected async Task<Guid?> GetCountryKeyByName(string lookupString)
        {
            var query = await Provider.SearchCountries(lookupString, DateTime.Now);

            if (!query.Any())
                return null;

            return query.First().MainLinkData.HeaderKey;
        }
    }
}
