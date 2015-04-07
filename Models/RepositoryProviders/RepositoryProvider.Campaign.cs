using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Standard.Campaigns;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Campaign> campaignRepository;
        private DbSet<Campaign> CampaignRepository
        {
            get { return campaignRepository = campaignRepository ?? context.Set<Campaign>(); }
        }

        private IQueryable<Campaign> Campaigns
        {
            get { return CampaignRepository as IQueryable<Campaign>; }
        }

        public void Add(Campaign campaign)
        {
            CampaignRepository.Add(campaign);
        }

        public void Remove(Campaign campaign)
        {
            CampaignRepository.Remove(campaign);
        }

        public async Task<Campaign> FindCampaignAsync(Guid competitionKey, DateTime viewDate)
        {
            return await Campaigns.SingleOrDefaultAsync(c => c.CompetitionKey == competitionKey && c.StartDate <= viewDate && c.EndDate >= viewDate);
        }

        public async Task<Campaign> GetCampaign(Guid campaignKey)
        {
            return await Campaigns.SingleOrDefaultAsync(s => s.PrimaryKey == campaignKey);
        }

        public async Task<IEnumerable<CodePickerViewModel>> GetCompetitionPickerData()
        {
            var query = from cam in Campaigns
                        join com in CompetitionVs.Where(w => w.IsActive) on cam.CompetitionKey equals com.HeaderKey
                        select com;

            var competitions = await query.ToListAsync();

            return competitions.Select(s => new CodePickerViewModel() { Code = s.HeaderKey, Description = s.CompetitionName });
        }

        public async Task<IEnumerable<BaseCampaignViewModel>> GetEspnCampaigns()
        {
            var campaignViewModels = new List<BaseCampaignViewModel>();

            var competitions = await LookupCompetitions.Where(w => w.ImportSite == ImportSite.Espn).Select(s => s.Competition).ToListAsync();

            foreach (var competition in competitions)
            {
                var campaign = await FindCampaignAsync(competition.PrimaryKey, DateTime.Today);

                if (campaign != null)
                    campaignViewModels.Add(campaign.ToViewModel(DateTime.Today));
            }

            return campaignViewModels;
        }
    }
}
