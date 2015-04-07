using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Models.ViewModels.Standard;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<MatchEvent> matchEventRepository;
        private DbSet<MatchEvent> MatchEventRepository
        {
            get { return matchEventRepository = matchEventRepository ?? context.Set<MatchEvent>(); }
        }

        private IQueryable<MatchEvent> MatchEvents
        {
            get { return MatchEventRepository as IQueryable<MatchEvent>; }
        }

        public void Add(MatchEvent matchEvent)
        {
            MatchEventRepository.Add(matchEvent);
        }

        public void Remove(MatchEvent matchEvent)
        {
            MatchEventRepository.Remove(matchEvent);
        }

        public async Task<IEnumerable<MatchEvent>> GetMatchEventsByCampaignAsync(Guid campaignKey)
        {
            return await MatchEvents.Where(me => me.MatchV.CampaignStage.CampaignKey == campaignKey).OrderBy(me => me.PersonPrimaryKey).ToListAsync();
        }

        public async Task<IEnumerable<MatchEvent>> GetMatchEvents(Guid matchVKey, Guid teamKey, Guid personKey, MatchEventType matchEventType, short? minute, short? extra)
        {
            return await MatchEvents.Where(f =>
                f.MatchVPrimaryKey == matchVKey
                && f.TeamPrimaryKey == teamKey
                && f.PersonPrimaryKey == personKey
                && f.MatchEventType == matchEventType
                && f.Minute == minute
                && f.Extra == extra).ToListAsync();
        }

        public async Task<IEnumerable<MatchEventViewModel>> GetMatchEventsByPerson(Guid personKey, DateTime viewDate)
        {
            var matchEvents = await MatchEvents.Where(w => w.PersonPrimaryKey == personKey && w.MatchV.IsActive).OrderByDescending(o => o.MatchV.MatchDate).ToListAsync();

            return matchEvents.ToViewModels(viewDate);
        }

        public async Task<MatchEvent> GetMatchEventByPrimaryKey(Guid primaryKey)
        {
            return await MatchEvents.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey);
        }

        public async Task<IEnumerable<PersonMatchEventCountViewModel>> GetTopPersonCampaignEventTotalsByMatchEventType(Guid campaignKey, MatchEventType matchEventType, int topNumber = 5)
        {
            var query = from me in MatchEvents.Where(w => w.MatchEventType == matchEventType)
                        join mv in MatchVs.Where(w => w.CampaignStage.CampaignKey == campaignKey && w.IsActive) on me.MatchVPrimaryKey equals mv.PrimaryKey
                        group me by me.PersonPrimaryKey into meGroup
                        let count = meGroup.Count() 
                        let person = meGroup.FirstOrDefault().Person
                        orderby count descending
                        select new PersonMatchEventCountViewModel()
                        {
                            Person = person,
                            Count = count
                        };

            return await query.Take(topNumber).ToListAsync();
        }
    }
}
