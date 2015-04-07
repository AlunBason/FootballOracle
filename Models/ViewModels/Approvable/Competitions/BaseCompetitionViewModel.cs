using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using FootballOracle.Models.ViewModels.Base;
using FootballOracle.Models.ViewModels.Standard.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class BaseCompetitionViewModel : BaseApprovableViewModel<Competition, CompetitionV>, ISearchResult, IApprovableLinkData
    {
        #region Properties
        public AreaType AreaType
        {
            get { return AreaType.Cmp; }
        }

        public override Competition HeaderEntity
        {
            get { return VersionEntity.Competition; }
        }

        public IEnumerable<BaseCampaignViewModel> CampaignViewModels { get; set; }
        public BaseCampaignViewModel SelectedCampaignViewModel { get; set; }
        public DateTime DatePickerMin { get; set; }
        public DateTime DatePickerMax { get; set; }
        public bool HasChildCampaigns { get; set; }

        private BaseOrganisationViewModel organisationViewModel;
        public BaseOrganisationViewModel OrganisationViewModel
        {
            get { return organisationViewModel = organisationViewModel ?? VersionEntity.Organisation.ToViewModel(ViewDate); }
        }

        private IEnumerable<DateRangePickerViewModel> campaignPickerData;
        public IEnumerable<DateRangePickerViewModel> CampaignPickerData
        {
            get { return campaignPickerData = campaignPickerData ?? HeaderEntity.ToDateRangePickerViewModels(); }
        }

        public string SelectedCampaignDate { get; set; }

        public ImportCampaignViewModel ImportCampaignViewModel { get; set; }


        public IApprovableLinkData ParentLinkData
        {
            get { return VersionEntity.OrganisationGuid != null ? OrganisationViewModel : null; ; }
        }

        private BaseCountryViewModel countryViewModel;
        public BaseCountryViewModel CountryViewModel
        {
            get
            {
                if (countryViewModel == null)
                {
                    var parentCountry = VersionEntity.Organisation.GetParentCountry(ViewDate);

                    if (parentCountry == null)
                        return null;

                    countryViewModel = parentCountry.ToViewModel(ViewDate);
                }

                return countryViewModel;
            }
        }
        #endregion

        #region Methods
        public static BaseCompetitionViewModel Create()
        {
            return Create(Guid.NewGuid());
        }

        public static BaseCompetitionViewModel Create(Guid headerGuid)
        {
            return new BaseCompetitionViewModel()
            {
                HeaderKey = headerGuid,
                EffectiveFrom = Date.LowDate,
                EffectiveTo = Date.HighDate
            };
        }

        public override string ToString()
        {
            return VersionEntity.CompetitionName;
        }

        public void SetCampaigns(int viewType = 3, Guid? campaignStageKey = null)
        {
            CampaignViewModels = HeaderEntity.Campaigns != null ? HeaderEntity.Campaigns.ToViewModels(ViewDate) : new List<BaseCampaignViewModel>();

            if (CampaignViewModels.Any())
            {
                SelectedCampaignViewModel = CampaignViewModels.SingleOrDefault(s => s.Entity.StartDate <= ViewDate && s.Entity.EndDate >= ViewDate);

                DatePickerMin = CampaignViewModels.OrderBy(o => o.Entity.EndDate).First().Entity.StartDate;
                DatePickerMax = CampaignViewModels.OrderByDescending(o => o.Entity.EndDate).First().Entity.EndDate;

                if (SelectedCampaignViewModel != null)
                {
                    SelectedCampaignViewModel.SetCampaignStages(viewType, campaignStageKey);
                    SelectedCampaignDate = SelectedCampaignViewModel.Entity.EndDate.ToUrlString();
                }
            }
        }
        #endregion
    }
}