using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionVersionSummaryViewModel : CompetitionEditorViewModel
    {
        [Display(Name = "Name")]
        [ReadOnly(true)]
        public override string CompetitionName { get; set; }

        [Display(Name = "Competition type")]
        [ReadOnly(true)]
        public string CompetitionTypeName
        {
            get { return CompetitionType.ToString(); }
        }

        [Display(Name = "Organisation")]
        [ReadOnly(true)]
        public string Organisation
        {
            get { return OrganisationViewModel.ToString(); }
        }

        [ReadOnly(true)]
        public override int? Rank { get; set; }

        [Display(Name = "Url")]
        [ReadOnly(true)]
        public override string WebAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective from")]
        [ReadOnly(true)]
        public override DateTime EffectiveFrom { get; set; }
    }
}
