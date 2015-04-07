using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<CampaignStage> campaignStageRepository;
        private DbSet<CampaignStage> CampaignStageRepository
        {
            get { return campaignStageRepository = campaignStageRepository ?? context.Set<CampaignStage>(); }
        }

        private IQueryable<CampaignStage> CampaignStages
        {
            get { return CampaignStageRepository as IQueryable<CampaignStage>; }
        }

        public void Add(CampaignStage campaignStage)
        {
            CampaignStageRepository.Add(campaignStage);
        }

        public void Remove(CampaignStage campaignStage)
        {
            CampaignStageRepository.Remove(campaignStage);
        }

        public async Task<CampaignStage> GetCampaignStage(Guid primaryKey)
        {
            return await CampaignStages.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey);
        }
    }
}
