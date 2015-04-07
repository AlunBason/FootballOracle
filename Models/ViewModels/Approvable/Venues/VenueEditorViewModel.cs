using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FootballOracle.Foundation.Entities;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.ViewModels.Approvable.Venues
{
    public class VenueEditorViewModel : BaseVenueViewModel, IEquatable<VenueV>
    {
        #region EditorProperties
        [Display(Name = "Name")]
        public virtual string VenueName { get; set; }

        public int? Capacity { get; set; }

        [Display(Name = "Address 1")]
        public virtual string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public virtual string Address2 { get; set; }

        [Display(Name = "Town/City")]
        public virtual string Address3 { get; set; }

        [Display(Name = "Region")]
        public virtual string Address4 { get; set; }

        [Display(Name = "Post code")]
        public virtual string PostCode { get; set; }

        [UIHint("DropDowns/Country")]
        [Display(Name = "Country")]
        public Guid CountryGuid { get; set; }
        #endregion

        public IEnumerable<BaseVenueViewModel> EditableViewModels { get; set; }

        public void GetEntityData()
        {
            VenueName = VersionEntity.VenueName;
            Capacity = VersionEntity.Capacity;
            Address1 = VersionEntity.Address1;
            Address2 = VersionEntity.Address2;
            Address3 = VersionEntity.Address3;
            Address4 = VersionEntity.Address4;
            PostCode = VersionEntity.PostCode;
            CountryGuid = VersionEntity.CountryGuid;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public bool Equals(VenueV other)
        {
            return VenueName == other.VenueName
                && Capacity == other.Capacity
                && Address1 == other.Address1
                && Address2 == other.Address2
                && Address3 == other.Address3
                && Address4 == other.Address4
                && PostCode == other.PostCode
                && CountryGuid == other.CountryGuid;
        }
    }

    public static class VenueEditorViewModelExtensions
    {
        public static VenueV ToVenueV(this VenueEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<VenueV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            newEntityV.VenueName = editorViewModel.VenueName;
            newEntityV.Capacity = editorViewModel.Capacity;
            newEntityV.Address1 = editorViewModel.Address1;
            newEntityV.Address2 = editorViewModel.Address2;
            newEntityV.Address3 = editorViewModel.Address3;
            newEntityV.Address4 = editorViewModel.Address4;
            newEntityV.PostCode = editorViewModel.PostCode;
            newEntityV.CountryGuid = editorViewModel.CountryGuid;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = editorViewModel.EffectiveFrom;
            newEntityV.EffectiveTo = editorViewModel.EffectiveTo;

            return newEntityV;
        }
    }
}
