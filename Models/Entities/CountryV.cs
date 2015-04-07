using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class CountryV : BaseApprovableEntity
    {
        [Required]
        [StringLength(50)]
        public string CountryName { get; set; }
        
        public Guid OrganisationGuid { get; set; }
                
        #region Navigation properties
        public virtual Country Country { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual Organisation Organisation { get; set; }
        #endregion
    }

    public static class _CountryVExtensions
    {
        public static IEnumerable<BaseCountryViewModel> ToViewModels(this IEnumerable<CountryV> versions, DateTime viewDate)
        {
            foreach (var version in versions)
                yield return version.ToViewModel(viewDate);
        }

        public static BaseCountryViewModel ToViewModel(this CountryV version, DateTime viewDate)
        {
            return version.ToViewModel<BaseCountryViewModel, Country, CountryV>(viewDate);
        }

        public static CountryV SetData(this CountryV entity, CountryEditorViewModel viewModel)
        {
            entity.HeaderKey = viewModel.HeaderKey;
            entity.CountryName = viewModel.CountryName;
            entity.ResourceGuid = viewModel.ResourceGuid;
            entity.EffectiveFrom = viewModel.EffectiveFrom;
            entity.EffectiveTo = viewModel.EffectiveTo;

            return entity;
        }
    }
}
