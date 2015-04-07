using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.People
{
    public class PersonVersionSummaryViewModel : PersonEditorViewModel
    {
        [ReadOnly(true)]
        public override string Forenames { get; set; }

        [ReadOnly(true)]
        public override string Surname { get; set; }

        [Display(Name = "Date of birth")]
        [ReadOnly(true)]
        public override DateTime? DateOfBirth { get; set; }

        [Display(Name = "Date of death")]
        [ReadOnly(true)]
        public override DateTime? DateOfDeath { get; set; }

        [Display(Name = "Country")]
        [ReadOnly(true)]
        public override Guid? CountryGuid { get; set; }

        [ReadOnly(true)]
        public string Country
        {
            get { return CountryViewModel.ToString(); }
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
