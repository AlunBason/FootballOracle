using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class VenueV : BaseApprovableEntity
    {
        [Required]
        [StringLength(100)]
        public string VenueName { get; set; }

        public int? Capacity { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string Address3 { get; set; }

        [StringLength(100)]
        public string Address4 { get; set; }

        [StringLength(100)]
        public string PostCode { get; set; }

        [Required]
        public Guid CountryGuid { get; set; }

        #region Navigation properties
        public virtual Venue Venue { get; set; }
        public virtual Country Country { get; set; }
        #endregion
    }

    public static class VenueVExtensions
    {
        public static IEnumerable<BaseVenueViewModel> ToViewModels(this IEnumerable<VenueV> versions, DateTime viewDate)
        {
            foreach (var version in versions)
                yield return version.ToViewModel(viewDate);
        }

        public static BaseVenueViewModel ToViewModel(this VenueV version, DateTime viewDate)
        {
            return version.ToViewModel<BaseVenueViewModel, Venue, VenueV>(viewDate);
        }

        public static void SetData(this VenueV entityV, VenueEditorViewModel viewModel)
        {
            entityV.VenueName = viewModel.VenueName;
            entityV.Capacity = viewModel.Capacity;
            entityV.Address1 = viewModel.Address1;
            entityV.Address2 = viewModel.Address2;
            entityV.Address3 = viewModel.Address3;
            entityV.Address4 = viewModel.Address4;
            entityV.PostCode = viewModel.PostCode;
            entityV.CountryGuid = viewModel.CountryGuid;
        }
    }
}
