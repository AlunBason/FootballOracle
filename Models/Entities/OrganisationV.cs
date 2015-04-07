using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class OrganisationV : BaseApprovableEntity
    {
        [Required]
        [StringLength(50)]
        public string OrganisationName { get; set; }

        [StringLength(100)]
        public string OrganisationDescription { get; set; }
        
        public Guid? ParentOrganisationGuid { get; set; }
        
        public Guid? CountryGuid { get; set; }

        #region Navigation properties
        public virtual Organisation Organisation { get; set; }
        public virtual Organisation ParentOrganisation { get; set; }
        public virtual Country Country { get; set; }
        #endregion
    }

    public static class _OrganisationVExtensions
    {
        public static IEnumerable<BaseOrganisationViewModel> ToViewModels(this IEnumerable<OrganisationV> versions, DateTime viewDate)
        {
            foreach (var version in versions)
                yield return version.ToViewModel(viewDate);
        }

        public static BaseOrganisationViewModel ToViewModel(this OrganisationV version, DateTime viewDate)
        {
            return version.ToViewModel<BaseOrganisationViewModel, Organisation, OrganisationV>(viewDate);
        }

        public static OrganisationV SetData(this OrganisationV entity, OrganisationEditorViewModel viewModel)
        {
            entity.OrganisationName = viewModel.OrganisationName;
            entity.OrganisationDescription = viewModel.OrganisationDescription;
            entity.ParentOrganisationGuid = viewModel.ParentOrganisationGuid;
            entity.CountryGuid = viewModel.CountryGuid;
            entity.WebAddress = viewModel.WebAddress;

            return entity;
        }

        
    }
}
