using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(MatchV matchV);
        void Remove(MatchV matchV);
        Task<MatchV> GetMatch(Guid matchKey, DateTime viewDate);
        Task<MatchV> GetMatch(Guid primaryKey, Guid matchKey);
        Task<MatchV> GetMatchByCampaignAndTeams(Guid campaignKey, Guid team1Key, Guid team2Key, DateTime matchDate);
        Task<MatchV> GetMatchByTeams(Guid team1Key, Guid team2Key, DateTime matchDate);
        Task<IEnumerable<BaseCampaignViewModel>> GetTeamCampaigns(Guid teamKey, DateTime viewDate);
        Task<IEnumerable<BaseMatchViewModel>> GetMatchesWithoutVenues(Guid campaignKey, DateTime viewDate);
        Task<IEnumerable<DateTime>> GetMatchDatesWithoutEvents(Guid campaignKey);
        Task<IEnumerable<BaseMatchViewModel>> GetMatchResultsByVenue(Guid venueKey, DateTime viewDate);
        Task<IEnumerable<BaseMatchViewModel>> GetHeadToHeadMatches(Guid team1Key, Guid team2Key, DateTime viewDate);
        Task<IEnumerable<BaseMatchViewModel>> GetMatchesByDate(DateTime viewDate, SearchDirection searchDirection);
        Task<IEnumerable<BaseMatchViewModel>> GetMatchesByCompetitionAndDate(Guid competitionKey, DateTime matchDate);
        Task<IEnumerable<BaseMatchViewModel>> GetUnresolvedMatches();
        Task<IEnumerable<BaseMatchViewModel>> GetTeamMatchesByDate(DateTime startDate, DateTime endDate, Guid teamKey, DateTime viewDate);
    }
}
