using System;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Website.Controllers;
using System.Linq;
using FootballOracle.Foundation.Interfaces;

namespace FootballOracle.Website.Areas.Mtc.Controllers
{
    public abstract class BaseMatchController : ApprovableController<Match, MatchV>
    {
        #region Constructor
        public BaseMatchController(IRepositoryProvider provider)
            : base(provider, AreaType.Mtc)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetMatch(headerKey, viewDate)).ToViewModel<TViewModel, Match, MatchV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetMatch(primaryKey, headerKey)).ToViewModel<TViewModel, Match, MatchV>(DateTime.Now); ;
        }
    }
}