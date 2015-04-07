using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<LookupCampaignStage> lookupCampaignStageRepository;
        private DbSet<LookupCampaignStage> LookupCampaignStageRepository
        {
            get { return lookupCampaignStageRepository = lookupCampaignStageRepository ?? context.Set<LookupCampaignStage>(); }
        }

        private IQueryable<LookupCampaignStage> LookupCampaignStages
        {
            get { return LookupCampaignStageRepository as IQueryable<LookupCampaignStage>; }
        }

        public void Add(LookupCampaignStage lookupCampaignStage)
        {
            LookupCampaignStageRepository.Add(lookupCampaignStage);
        }

        public void Remove(LookupCampaignStage lookupCampaignStage)
        {
            LookupCampaignStageRepository.Remove(lookupCampaignStage);
        }

        public async Task<LookupCampaignStage> GetLookupCampaignStage(Guid primaryKey)
        {
            return await LookupCampaignStages.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey);
        }
    }
}
