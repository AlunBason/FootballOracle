using FootballOracle.Foundation.Entities;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.Organisations
{
    public class OrganisationEditorViewModel : BaseOrganisationViewModel, IEquatable<OrganisationV>
    {
        [StringLength(50)]
        [Required]
        [Display(Name = "Name")]
        public virtual string OrganisationName { get; set; }

        [StringLength(100)]
        [Required]
        [Display(Name = "Description")]
        public virtual string OrganisationDescription { get; set; }

        [UIHint("DropDowns/Organisation")]
        [Display(Name = "Parent")]
        //[ReadOnly(true)]
        public Guid? ParentOrganisationGuid { get; set; }

        [UIHint("DropDowns/Country")]
        [Display(Name = "Country")]
        //[ReadOnly(true)]
        public virtual Guid? CountryGuid { get; set; }

        public IEnumerable<BaseOrganisationViewModel> EditableViewModels { get; set; }

        public void GetEntityData()
        {
            OrganisationName = VersionEntity.OrganisationName;
            OrganisationDescription = VersionEntity.OrganisationDescription;
            ParentOrganisationGuid = VersionEntity.ParentOrganisationGuid;
            CountryGuid = VersionEntity.CountryGuid;
            WebAddress = VersionEntity.WebAddress;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public bool Equals(OrganisationV other)
        {
            return OrganisationName == other.OrganisationName
                && OrganisationDescription == other.OrganisationDescription
                && ParentOrganisationGuid == other.ParentOrganisationGuid
                && CountryGuid == other.CountryGuid
                && WebAddress == other.WebAddress;
        }
    }

    public static class _OrganisationEditorViewModelExtensions
    {
        public static OrganisationV ToOrganisationV(this OrganisationEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<OrganisationV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            newEntityV.OrganisationName = editorViewModel.OrganisationName;
            newEntityV.OrganisationDescription = editorViewModel.OrganisationDescription;
            newEntityV.ParentOrganisationGuid = editorViewModel.ParentOrganisationGuid;
            newEntityV.CountryGuid = editorViewModel.CountryGuid;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = editorViewModel.EffectiveFrom;

            return newEntityV;
        }
    }
}
