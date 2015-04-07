using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(MatchEvent matchEvent);
        void Remove(MatchEvent matchEvent);

        Task<IEnumerable<MatchEvent>> GetMatchEventsByCampaignAsync(Guid campaignKey);
        Task<IEnumerable<MatchEvent>> GetMatchEvents(Guid matchVKey, Guid teamKey, Guid personKey, MatchEventType matchEventType, short? minute, short? extra);
        Task<IEnumerable<MatchEventViewModel>> GetMatchEventsByPerson(Guid personKey, DateTime viewDate);
        Task<MatchEvent> GetMatchEventByPrimaryKey(Guid primaryKey);
        Task<IEnumerable<PersonMatchEventCountViewModel>> GetTopPersonCampaignEventTotalsByMatchEventType(Guid campaignKey, MatchEventType matchEventType, int topNumber = 5);
    }
}
