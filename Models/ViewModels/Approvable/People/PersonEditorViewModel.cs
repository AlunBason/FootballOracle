using FootballOracle.Foundation.Entities;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Approvable.People
{
    public class PersonEditorViewModel : BasePersonViewModel , IEquatable<PersonV>
    {
        #region EditorProperties
        public virtual string Forenames { get; set; }
        public virtual string Surname { get; set; }

        [UIHint("Date")]
        [Display(Name = "Date of birth")]
        public virtual DateTime? DateOfBirth { get; set; }

        [UIHint("Date")]
        [Display(Name = "Date of death")]
        public virtual DateTime? DateOfDeath { get; set; }

        [UIHint("DropDowns/Country")]
        [Display(Name = "Country")]
        public virtual Guid? CountryGuid { get; set; }

        [Display(Name = "Height (cm)")]
        public virtual int?  Height { get; set; }

        #endregion

        public IEnumerable<BasePersonViewModel> EditableViewModels { get; set; }

        public void GetEntityData()
        {
            Forenames = VersionEntity.Forenames;
            Surname = VersionEntity.Surname;
            DateOfBirth = VersionEntity.DateOfBirth;
            DateOfDeath = VersionEntity.DateOfDeath;
            CountryGuid = VersionEntity.CountryGuid;
            Height = VersionEntity.Height;
            WebAddress = VersionEntity.WebAddress;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public bool Equals(PersonV other)
        {
            return Forenames == other.Forenames
                && Surname == other.Surname
                && DateOfBirth == other.DateOfBirth
                && DateOfDeath == other.DateOfDeath
                && CountryGuid == other.CountryGuid
                && Height == other.Height
                && WebAddress == other.WebAddress;
        }
    }

    public static class PersonEditorViewModelExtensions
    {
        public static PersonV ToPersonV(this PersonEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<PersonV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            newEntityV.Forenames = editorViewModel.Forenames;
            newEntityV.Surname = editorViewModel.Surname;
            newEntityV.SearchText = newEntityV.SetSearchText();
            newEntityV.DateOfBirth = editorViewModel.DateOfBirth;
            newEntityV.DateOfDeath = editorViewModel.DateOfDeath;
            newEntityV.CountryGuid = editorViewModel.CountryGuid;
            newEntityV.Height = editorViewModel.Height;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = editorViewModel.EffectiveFrom;
            newEntityV.EffectiveTo = editorViewModel.EffectiveTo;

            return newEntityV;
        }
    }
}
