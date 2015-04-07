using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(Campaign campaign);
        void Remove(Campaign campaign);
        Task<Campaign> FindCampaignAsync(Guid competitionKey, DateTime viewDate);
        Task<Campaign> GetCampaign(Guid campaignKey);
        Task<IEnumerable<CodePickerViewModel>> GetCompetitionPickerData();
        Task<IEnumerable<BaseCampaignViewModel>> GetEspnCampaigns();
    }
}
