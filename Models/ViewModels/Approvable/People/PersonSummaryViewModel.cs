using FootballOracle.Foundation;
using FootballOracle.Models.ViewModels.Standard;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.ViewModels.Approvable.People
{
    public class PersonSummaryViewModel : BasePersonViewModel
    {
        #region Properties
        public IEnumerable<MatchEventViewModel> MatchEventViewModels { get; private set; }

        private IEnumerable<BaseCampaignViewModel> leagueCampaignViewModels;
        public IEnumerable<BaseCampaignViewModel> LeagueCampaignViewModels
        {
            get
            {
                if (leagueCampaignViewModels == null)
                {
                    leagueCampaignViewModels = MatchEventViewModels.Select(s => s.MatchViewModel)
                        .GroupBy(g => g.CampaignViewModel.Entity.PrimaryKey).Select(s => s.FirstOrDefault().CampaignViewModel).Distinct()
                        .OrderByDescending(o => o.Entity.StartDate);
                }

                return leagueCampaignViewModels;

            }
        }

        private BaseCampaignViewModel selectedLeagueCampaignViewModel;
        public BaseCampaignViewModel SelectedLeagueCampaignViewModel
        {
            get
            {
                if (selectedLeagueCampaignViewModel == null)
                {
                    selectedLeagueCampaignViewModel = LeagueCampaignViewModels.FirstOrDefault(w => w.Entity.StartDate <= ViewDate && w.Entity.EndDate >= ViewDate);

                    if (selectedLeagueCampaignViewModel == null)
                        selectedLeagueCampaignViewModel = LeagueCampaignViewModels.FirstOrDefault();
                }

                return selectedLeagueCampaignViewModel;
            }
        }

        private IEnumerable<MatchEventViewModel> selectedMatchEvents;
        public IEnumerable<MatchEventViewModel> SelectedMatchEvents 
        {
            get
            {
                if (selectedMatchEvents == null && SelectedLeagueCampaignViewModel != null)
                {
                    var query = from m in MatchEventViewModels
                                where m.MatchViewModel.MatchDate >= SelectedLeagueCampaignViewModel.Entity.StartDate
                                && m.MatchViewModel.MatchDate <= SelectedLeagueCampaignViewModel.Entity.EndDate
                                select m;

                    selectedMatchEvents = query.ToList();
                }

                return selectedMatchEvents;
            }
        }
        #endregion

        #region Methods
        public async Task SetMatchEventViewModels()
        {
            MatchEventViewModels = await DbProvider.GetMatchEventsByPerson(HeaderKey, ViewDate);
        }

        public IEnumerable<MatchEventViewModel> GetSeasonMatchEvents(BaseCampaignViewModel leagueCampaignViewModel)
        {
            var query = MatchEventViewModels
                .Where(w => w.MatchViewModel.MatchDate >= leagueCampaignViewModel.Entity.StartDate && w.MatchViewModel.MatchDate <= leagueCampaignViewModel.Entity.EndDate);

            return query.ToList();
        }
        #endregion
    }
}
