using System;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Website.Controllers;

namespace FootballOracle.Website.Areas.Ven.Controllers
{
    public abstract class BaseVenueController : ApprovableController<Venue, VenueV>
    {
        #region Constructor
        public BaseVenueController(IRepositoryProvider provider)
            : base(provider, AreaType.Ven)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetVenue(headerKey, viewDate)).ToViewModel<TViewModel, Venue, VenueV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetVenue(primaryKey, headerKey)).ToViewModel<TViewModel, Venue, VenueV>(DateTime.Now); ;
        }
    }
}