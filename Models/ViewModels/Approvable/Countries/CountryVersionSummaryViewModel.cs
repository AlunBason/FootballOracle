using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class CountryVersionSummaryViewModel : CountryEditorViewModel
    {
        [Display(Name = "Country name")]
        [ReadOnly(true)]
        public override string CountryName { get; set; }

        [Display(Name = "Organisation")]
        [ReadOnly(true)]
        public string Organisation
        {
            get { return OrganisationViewModel != null ? OrganisationViewModel.ToString() : string.Empty; }
        }

        [Display(Name = "Url")]
        [ReadOnly(true)]
        public override string WebAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective from")]
        [ReadOnly(true)]
        public override DateTime EffectiveFrom { get; set; }
    }
}
