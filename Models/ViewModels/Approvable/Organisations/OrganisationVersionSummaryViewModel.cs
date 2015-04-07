using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.Organisations
{
    public class OrganisationVersionSummaryViewModel : OrganisationEditorViewModel
    {
        [Display(Name = "Name")]
        [ReadOnly(true)]
        public override string OrganisationName { get; set; }

        [Display(Name = "Description")]
        [ReadOnly(true)]
        public override string OrganisationDescription { get; set; }

        [Display(Name = "Parent")]
        [ReadOnly(true)]
        public string ParentOrganisation 
        {
            get { return ParentOrganisationGuid != null ? ParentOrganisationViewModel.ToString() : string.Empty; }
        }

        [Display(Name = "Country")]
        [ReadOnly(true)]
        public string Country
        {
            get { return CountryGuid != null ? CountryViewModel.ToString() : string.Empty; }
        }

        [UIHint("DropDowns/Country")]
        [Display(Name = "Country")]
        [ReadOnly(true)]
        public override Guid? CountryGuid { get; set; }

        [Display(Name = "Url")]
        [ReadOnly(true)]
        public override string WebAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective from")]
        [ReadOnly(true)]
        public override DateTime EffectiveFrom { get; set; }
    }
}
