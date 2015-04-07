using System;
using System.Threading.Tasks;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(CampaignStage campaignStage);
        void Remove(CampaignStage campaignStage);
        Task<CampaignStage> GetCampaignStage(Guid primaryKey);
    }
}
