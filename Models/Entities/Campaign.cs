using FootballOracle.Foundation.Entities;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.Entities
{
    public class Campaign : BaseEntity
    {
        public Campaign()
        {
            CampaignStages = new HashSet<CampaignStage>();
        }

        public Guid CompetitionKey { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        #region Navigation properties
        public virtual Competition Competition { get; set; }
        public virtual ICollection<CampaignStage> CampaignStages { get; set; }
        #endregion

        public static Campaign CreateNew(Guid competitionKey, DateTime startDate, DateTime endDate)
        {
            return new Campaign()
            {
                PrimaryKey = Guid.NewGuid(),
                CompetitionKey = competitionKey,
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }

    public static class CampaignExtensions
    {
        public static BaseCampaignViewModel ToViewModel(this Campaign campaign, DateTime viewDate)
        {
            return new BaseCampaignViewModel()
            {
                Entity = campaign,
                ViewDate = viewDate
            };
        }

        public static IEnumerable<BaseCampaignViewModel> ToViewModels(this IEnumerable<Campaign> campaigns, DateTime viewDate)
        {
            foreach (var campaign in campaigns)
                yield return campaign.ToViewModel(viewDate);
        }
    }
}
