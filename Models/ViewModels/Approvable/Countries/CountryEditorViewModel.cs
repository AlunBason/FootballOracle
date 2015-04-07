using FootballOracle.Foundation.Entities;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class CountryEditorViewModel : BaseCountryViewModel, IEquatable<CountryV>
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Country name")]
        [StringLength(100)]
        public virtual new string CountryName { get; set; }

        [Display(Name = "Organisation")]
        [UIHint("DropDowns/Organisation")]
        public Guid OrganisationGuid { get; set; }

        [Display(Name = "Image")]
        [UIHint("Image")]
        public Guid? ResourceGuid { get; set; }

        public IEnumerable<BaseCountryViewModel> EditableViewModels { get; set; }

        public void GetEntityData()
        {
            CountryName = VersionEntity.CountryName;
            ResourceGuid = VersionEntity.ResourceGuid;
            OrganisationGuid = VersionEntity.OrganisationGuid;
            WebAddress = VersionEntity.WebAddress;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public new bool Equals(CountryV other)
        {
            return CountryName == other.CountryName
                && OrganisationGuid == other.OrganisationGuid
                && ResourceGuid == other.ResourceGuid
                && WebAddress == other.WebAddress;
        }
    }

    public static class _CountryEditorViewModelExtensions
    {
        public static CountryV ToCountryV(this CountryEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<CountryV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            newEntityV.CountryName = editorViewModel.CountryName;
            newEntityV.OrganisationGuid = editorViewModel.OrganisationGuid;
            newEntityV.ResourceGuid = editorViewModel.ResourceGuid;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = editorViewModel.EffectiveFrom;
            newEntityV.EffectiveTo = editorViewModel.EffectiveTo;

            return newEntityV;
        }
    }
}
