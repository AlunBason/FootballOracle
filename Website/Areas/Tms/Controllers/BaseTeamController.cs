using System;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Website.Controllers;

namespace FootballOracle.Website.Areas.Tms.Controllers
{
    public class BaseTeamController : ApprovableController<Team, TeamV>
    {
        #region Constructor
        public BaseTeamController(IRepositoryProvider provider)
            : base(provider, AreaType.Tms)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetTeam(headerKey, viewDate)).ToViewModel<TViewModel, Team, TeamV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetTeam(primaryKey, headerKey)).ToViewModel<TViewModel, Team, TeamV>(DateTime.Now); ;
        }
    }
}