using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Models.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class BaseCountryViewModel : BaseApprovableViewModel<Country, CountryV>, IEquatable<CountryV>, ISearchResult, IApprovableLinkData
    {
        public override string ToString()
        {
            return VersionEntity != null ? VersionEntity.CountryName : string.Empty;
        }

        #region Editor properties
        [Required]
        [Display(Name = "Country name")]
        public string CountryName { get; set; }
        #endregion

        public bool HasChildOrganisations { get; set; }
        public bool HasChildTeams { get; set; }
        public bool HasChildVenues { get; set; }

        public BaseOrganisationViewModel OrganisationViewModel
        {
            get { return VersionEntity.Organisation.ToViewModel(ViewDate); }
        }

        #region IEquatable implementation
        public bool Equals(CountryV other)
        {
            return CountryName == other.CountryName
                && EffectiveFrom == other.EffectiveFrom
                && EffectiveTo == other.EffectiveTo;
        }
        #endregion
        
        public static BaseCountryViewModel Create()
        {
            return Create(Guid.NewGuid());
        }

        public static BaseCountryViewModel Create(Guid headerGuid)
        {
            return new BaseCountryViewModel()
            {
                HeaderKey = headerGuid,
                EffectiveFrom = Date.LowDate,
                EffectiveTo = Date.HighDate
            };
        }

        public IApprovableLinkData ParentLinkData
        {
            get 
            {
                if (VersionEntity.Organisation != null)
                    return OrganisationViewModel;

                return null;
            }
        }

        private IEnumerable<BaseTeamViewModel> teamViewModels;
        public IEnumerable<BaseTeamViewModel> TeamViewModels
        {
            get { return teamViewModels = teamViewModels ?? HeaderEntity.TeamVs.Where(e => e.IsActive && !e.IsMarkedForDeletion && e.EffectiveFrom <= ViewDate && e.EffectiveTo >= ViewDate).ToViewModels(ViewDate); }
        }

        public AreaType AreaType
        {
            get { return AreaType.Cnt; }
        }

        public override byte[] ResourceBytes
        {
            get { return VersionEntity.Resource != null ? VersionEntity.Resource.ResourceBytes : null; }
        }

        public override Country HeaderEntity
        {
            get { return VersionEntity.Country; }
        }
    }
}