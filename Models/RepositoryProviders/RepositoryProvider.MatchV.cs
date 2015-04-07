using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<MatchV> matchVRepository;
        private DbSet<MatchV> MatchVRepository
        {
            get { return matchVRepository = matchVRepository ?? context.Set<MatchV>(); }
        }

        private IQueryable<MatchV> MatchVs
        {
            get { return MatchVRepository as IQueryable<MatchV>; }
        }

        public void Add(MatchV matchV)
        {
            MatchVRepository.Add(matchV);
        }

        public void Remove(MatchV matchV)
        {
            MatchVRepository.Remove(matchV);
        }

        public async Task<MatchV> GetMatch(Guid matchKey, DateTime viewDate)
        {
            return (await Matches.SingleOrDefaultAsync(f => f.PrimaryKey == matchKey)).GetApprovedVersion(viewDate);
        }

        public async Task<MatchV> GetMatch(Guid primaryKey, Guid matchKey)
        {
            return await MatchVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == matchKey);
        }

        public async Task<IEnumerable<MatchV>> GetCampaignMatches(Guid campaignKey)
        {
            return await MatchVs.Where(m => m.CampaignStage.CampaignKey == campaignKey).ToListAsync();
        }

        public async Task<IEnumerable<MatchV>> GetCampaignFixtures(Guid campaignKey)
        {
            return (await GetCampaignMatches(campaignKey)).Where(w => w.Team1FT == null && w.Team2FT == null);
        }

        public async Task<IEnumerable<MatchV>> GetCampaignResults(Guid campaignKey, DateTime viewDate)
        {
            var endOfViewDate = viewDate.ToEndOfDay();

            return (await GetCampaignMatches(campaignKey)).Where(m => m.Team1FT != null && m.Team2FT != null && m.MatchDate <= endOfViewDate);
        }

        public async Task<IEnumerable<BaseCampaignViewModel>> GetTeamCampaigns(Guid teamKey, DateTime viewDate)
        {
            var campaigns = await MatchVs.Where(w => w.Team1Guid == teamKey || w.Team2Guid == teamKey).Select(m => m.CampaignStage.Campaign).ToListAsync();

            return campaigns.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetMatchesWithoutVenues(Guid campaignKey, DateTime viewDate)
        {
            var matches = await MatchVs.Where(w => w.CampaignStage.CampaignKey == campaignKey && w.VenueGuid == null).ToListAsync();

            return matches.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<DateTime>> GetMatchDatesWithoutEvents(Guid campaignKey)
        {
            return await MatchVs.Where(w => w.CampaignStage.CampaignKey == campaignKey && w.Team1FT != null && w.Team2FT != null && !w.MatchEvents.Any()).OrderBy(o => o.MatchDate).Select(s => s.MatchDate).Distinct().ToListAsync();
        }

        public async Task<MatchV> GetMatchByCampaignAndTeams(Guid campaignKey, Guid team1Key, Guid team2Key, DateTime matchDate)
        {
            var beginningOfMatchDay =  matchDate.Date;
            var endOfMatchDay = matchDate.ToEndOfDay();

            return await MatchVs.FirstOrDefaultAsync(f =>
                f.CampaignStage.CampaignKey == campaignKey
                && f.Team1Guid == team1Key
                && f.Team2Guid == team2Key
                && f.IsActive
                && !f.IsMarkedForDeletion
                && f.MatchDate >= beginningOfMatchDay
                && f.MatchDate <= endOfMatchDay);
        }

        public async Task<MatchV> GetMatchByTeams(Guid team1Key, Guid team2Key, DateTime matchDate)
        {
            var beginningOfMatchDay = matchDate.Date;
            var endOfMatchDay = matchDate.ToEndOfDay();

            return await MatchVs.FirstOrDefaultAsync(f =>
                f.Team1Guid == team1Key
                && f.Team2Guid == team2Key
                && f.MatchDate >= beginningOfMatchDay
                && f.MatchDate <= endOfMatchDay
                && f.IsActive
                && !f.IsMarkedForDeletion);
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetMatchResultsByVenue(Guid venueKey, DateTime viewDate)
        {
            var matchVs = await MatchVs.Where(w => w.VenueGuid == venueKey && w.IsActive && w.Team1FT != null && w.Team2FT != null).OrderByDescending(o => o.MatchDate).ToListAsync();

            return matchVs.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetHeadToHeadMatches(Guid team1Key, Guid team2Key, DateTime viewDate)
        {
            var query = from m in MatchVs
                        where ((m.Team1Guid == team1Key && m.Team2Guid == team2Key)
                        || (m.Team1Guid == team2Key && m.Team2Guid == team1Key))
                        && m.Team1FT != null && m.Team2FT != null
                        && m.IsActive && !m.IsMarkedForDeletion
                        orderby m.MatchDate descending
                        select m.Match;

            return (await query.ToListAsync()).ToViewModels(viewDate);
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetMatchesByDate(DateTime viewDate, SearchDirection searchDirection)
        {
            switch (searchDirection)
            {
                case SearchDirection.Current:
                    var matches = await MatchVs.Where(w => w.MatchDate == viewDate.Date).ToListAsync();
                    return matches.ToViewModels(viewDate).OrderBy(o => o.CompetitionViewModel.CountryViewModel != null ? o.CompetitionViewModel.CountryViewModel.ToString() : o.CompetitionViewModel.ToString()).ThenBy(t => t.Team1ViewModel.ToString());

                case SearchDirection.Down:
                    var matchV = await MatchVs.Where(w => w.MatchDate < viewDate.Date).OrderByDescending(o => o.MatchDate).FirstOrDefaultAsync();
                    matches = await MatchVs.Where(w => w.MatchDate == matchV.MatchDate).ToListAsync();
                    return matches.ToViewModels(viewDate).OrderBy(o => o.CompetitionViewModel.CountryViewModel != null ? o.CompetitionViewModel.CountryViewModel.ToString() : o.CompetitionViewModel.ToString()).ThenBy(t => t.Team1ViewModel.ToString());
                    
                case SearchDirection.Up:
                    matchV = await MatchVs.Where(w => w.MatchDate > viewDate.Date).OrderBy(o => o.MatchDate).FirstOrDefaultAsync();
                    matches = await MatchVs.Where(w => w.MatchDate == matchV.MatchDate).ToListAsync();
                    return matches.ToViewModels(viewDate).OrderBy(o => o.CompetitionViewModel.CountryViewModel != null ? o.CompetitionViewModel.CountryViewModel.ToString() : o.CompetitionViewModel.ToString()).ThenBy(t => t.Team1ViewModel.ToString());
            }

            return null;
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetMatchesByCompetitionAndDate(Guid competitionKey, DateTime matchDate)
        {
            var endOfDay = matchDate.ToEndOfDay();

            var query = from m in MatchVs
                        where m.CampaignStage.Campaign.CompetitionKey == competitionKey
                        && m.MatchDate >= matchDate.Date
                        && m.MatchDate <= endOfDay
                        && m.IsActive
                        && !m.IsMarkedForDeletion
                        select m;

            var matchVs = await query.ToListAsync();

            return matchVs.ToViewModels(matchDate).OrderBy(o => o.ToString());
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetUnresolvedMatches()
        {
            var query = from m in MatchVs
                        where m.MatchDate < DateTime.Now
                        && m.Team1FT == null
                        && m.Team2FT == null
                        select m;

            var matchVs = await query.ToListAsync();

            return matchVs.ToViewModels(DateTime.Now).OrderByDescending(o => o.MatchDate).ThenBy(t => t.Team1ViewModel.ToString());
        }

        public async Task<IEnumerable<BaseMatchViewModel>> GetTeamMatchesByDate(DateTime startDate, DateTime endDate, Guid teamKey, DateTime viewDate)
        {
            var query = from m in MatchVs
                        where m.MatchDate >= startDate
                        && m.MatchDate <= endDate
                        && (m.Team1Guid == teamKey || m.Team2Guid == teamKey)
                        && m.Team1FT != null && m.Team2FT != null
                        && m.IsActive
                        orderby m.MatchDate descending
                        select m.Match;

            var matches = await query.ToListAsync();

            return matches.Distinct().ToViewModels(viewDate);
        }
    }
}
