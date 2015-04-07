using System;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Website.Controllers;
using System.Linq;
using FootballOracle.Foundation.Interfaces;

namespace FootballOracle.Website.Areas.Cmp.Controllers
{
    public abstract class BaseCompetitionController : ApprovableController<Competition, CompetitionV>
    {
        #region Constructor
        public BaseCompetitionController(IRepositoryProvider provider)
            : base(provider, AreaType.Cmp)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetCompetition(headerKey, viewDate)).ToViewModel<TViewModel, Competition, CompetitionV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetCompetition(primaryKey, headerKey)).ToViewModel<TViewModel, Competition, CompetitionV>(DateTime.Now); ;
        }
    }
}