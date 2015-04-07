using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using System.Threading.Tasks;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using FootballOracle.Foundation;
using FootballOracle.Foundation.ViewModels;

namespace FootballOracle.Models.ViewModels.Approvable.Venues
{
    public class VenueSummaryViewModel : BaseVenueViewModel
    {
        public IEnumerable<BaseMatchViewModel> MatchViewModels { get; private set; }

        private IEnumerable<BaseCampaignViewModel> leagueCampaignViewModels;
        public IEnumerable<BaseCampaignViewModel> LeagueCampaignViewModels
        {
            get
            {
                if (leagueCampaignViewModels == null)
                {
                    leagueCampaignViewModels = MatchViewModels
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

        private IEnumerable<DateRangePickerViewModel> dateRangePickerViewModels;
        public IEnumerable<DateRangePickerViewModel> DateRangePickerViewModels
        {
            get 
            { 
                if (dateRangePickerViewModels == null)
                {
                    var query = from c in LeagueCampaignViewModels
                                select new DateRangePickerViewModel() 
                                { 
                                    EndDateString = c.Entity.EndDate.ToUrlString(),
                                    Description = c.ToString()
                                };

                    dateRangePickerViewModels = query.ToList();
                }

                return dateRangePickerViewModels;
            }
        }

        public string SelectedDateRangePickerViewModel { get; set; }

        private IEnumerable<BaseMatchViewModel> selectedMatchViewModels;
        public IEnumerable<BaseMatchViewModel> SelectedMatchViewModels
        {
            get
            {
                if (selectedMatchViewModels == null)
                {
                    selectedMatchViewModels = new List<BaseMatchViewModel>();

                    if (SelectedLeagueCampaignViewModel != null)
                    {
                        var query = from c in MatchViewModels
                                    where c.MatchDate >= SelectedLeagueCampaignViewModel.Entity.StartDate
                                    && c.MatchDate <= SelectedLeagueCampaignViewModel.Entity.EndDate
                                    && c.Team1Ft != null && c.Team2Ft != null
                                    orderby c.MatchDate descending
                                    select c;

                        selectedMatchViewModels = query.ToList();
                    }
                }

                return selectedMatchViewModels;
            }
        }

        #region Methods
        public async Task SetMatchViewModels()
        {
            MatchViewModels = await DbProvider.GetMatchResultsByVenue(HeaderKey, ViewDate);
        }

        public void SetSelectedDateRangePickerViewModel()
        {
            var leagueCampaignViewModel = LeagueCampaignViewModels.SingleOrDefault(w => w.Entity.StartDate <= ViewDate && w.Entity.EndDate >= ViewDate);

            if (leagueCampaignViewModel != null)
                SelectedDateRangePickerViewModel = leagueCampaignViewModel.Entity.EndDate.ToUrlString();
            else
            {
                if (DateRangePickerViewModels.Any())
                    SelectedDateRangePickerViewModel = DateRangePickerViewModels.First().EndDateString;
            }
        }
        #endregion
    }
}
