using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using System.Threading.Tasks;

namespace FootballOracle.Website.Controllers
{
    public class PartialController : BaseController
    {
         #region Constructor
        public PartialController(IRepositoryProvider provider)
            : base(provider)
        {
        }
        #endregion

        [OutputCache(Duration = 0)]
        public async Task<PartialViewResult> PeopleSearch(string tx, DateTime dt, int tn, string id, string hd)
        {
            var basePeopleViewModels = await DbProvider.SearchPeople(tx, dt);
            var matchPersonViewModels = new List<MatchPersonViewModel>();

            foreach (var item in basePeopleViewModels)
            {
                matchPersonViewModels.Add(new MatchPersonViewModel()
                {
                    PersonGuid = item.MainLinkData.HeaderKey,
                    PersonName = item.Description
                });
            }

            var matchPlayersEditorViewModel = new MatchPlayersEditorViewModel()
            {
                PrimaryKey = id.ToGuid(),
                HeaderKey = hd.ToGuid(),
                ViewDate = dt,
                TeamNumber = tn,
                NewMatchPersonViewModels = matchPersonViewModels
            };

            return PartialView("_MatchEditorPlayerSearch", matchPlayersEditorViewModel);
        }
	}
}