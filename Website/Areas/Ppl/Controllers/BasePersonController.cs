using System;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Website.Controllers;

namespace FootballOracle.Website.Areas.Ppl.Controllers
{
    public abstract class BasePersonController : ApprovableController<Person, PersonV>
    {
        #region Constructor
        public BasePersonController(IRepositoryProvider provider)
            : base(provider, AreaType.Ppl)
        {
        }
        #endregion

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid headerKey, DateTime viewDate)
        {
            return (await DbProvider.GetPerson(headerKey, viewDate)).ToViewModel<TViewModel, Person, PersonV>(viewDate);
        }

        protected override async Task<TViewModel> GetViewModel<TViewModel>(Guid primaryKey, Guid headerKey)
        {
            return (await DbProvider.GetPerson(primaryKey, headerKey)).ToViewModel<TViewModel, Person, PersonV>(DateTime.Now); ;
        }
    }
}