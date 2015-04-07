using System;
using System.Threading.Tasks;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(LookupCampaignStage lookupCampaignStage);
        void Remove(LookupCampaignStage lookupCampaignStage);
        Task<LookupCampaignStage> GetLookupCampaignStage(Guid primaryKey);
    }
}
