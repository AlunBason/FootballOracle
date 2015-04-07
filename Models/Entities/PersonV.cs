using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.People;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class PersonV : BaseApprovableEntity
    {
        [StringLength(50)]
        public string Forenames { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [ScaffoldColumn(false)]
        public string SearchText { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }
        
        public Guid? CountryGuid { get; set; }

        public int? Height { get; set; }
                
        #region Navigation properties
        public virtual Country Country { get; set; }
        public virtual Person Person { get; set; }
        #endregion
    }

    public static class _PersonVExtensions
    {
        public static IEnumerable<BasePersonViewModel> ToViewModels(this IEnumerable<PersonV> versions, DateTime viewDate)
        {
            foreach (var version in versions)
                yield return version.ToViewModel(viewDate);
        }

        public static BasePersonViewModel ToViewModel(this PersonV version, DateTime viewDate)
        {
            return version.ToViewModel<BasePersonViewModel, Person, PersonV>(viewDate);
        }

        public static IEnumerable<BasePersonViewModel> ToBasePeopleViewModels(this IQueryable<PersonV> personVs, Guid userId, bool isAdmin, DateTime viewDate, string searchText)
        {
            var people = from p in personVs
                         where (p.SearchText).Contains(searchText)
                         select p;

            return people.ToViewModels(viewDate);
        }

        public static IEnumerable<BasePersonViewModel> GetEditableVersions(this Person entity, DateTime viewDate)
        {
            return entity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(viewDate);
        }

        public static PersonV SetData(this PersonV entity, PersonEditorViewModel viewModel)
        {
            entity.Forenames = viewModel.Forenames;
            entity.Surname = viewModel.Surname;
            entity.SearchText = entity.SetSearchText();
            entity.DateOfBirth = viewModel.DateOfBirth;
            entity.DateOfDeath = viewModel.DateOfDeath;
            entity.CountryGuid = viewModel.CountryGuid;
            entity.Height = viewModel.Height;
            entity.WebAddress = viewModel.WebAddress;

            return entity;
        }

        public static string SetSearchText(this PersonV personV)
        {
            var fullName = string.IsNullOrEmpty(personV.Forenames) ? personV.Surname.Trim() : string.Format("{0} {1}", personV.Forenames, personV.Surname);

            return fullName.RemoveDiacritics();
        }
    }
}
