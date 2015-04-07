using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using FootballOracle.Models.ViewModels.Approvable.People;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using FootballOracle.Models.ViewModels.Standard;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using System.Threading.Tasks;
using FootballOracle.Foundation;

namespace FootballOracle.Website.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IRepositoryProvider provider)
            :base(provider)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Results(string id, DateTime? dt)
        {
            if (string.IsNullOrEmpty(id) || id.Length < 3)
                return RedirectToAction("Index");

            var normalizedText = id.RemoveDiacritics();

            var competitions = await DbProvider.SearchCompetitions(id, dt ?? DateTime.Now);
            var countries = await DbProvider.SearchCountries(id, dt ?? DateTime.Now);
            var organisations = await DbProvider.SearchOrganisations(id, dt ?? DateTime.Now);
            var people = await DbProvider.SearchPeople(id, dt ?? DateTime.Now);
            var teams = await DbProvider.SearchTeams(id, dt ?? DateTime.Now);
            var venues = await DbProvider.SearchVenues(id, dt ?? DateTime.Now);

            var homeViewModel = new HomeViewModel()
            {
                SearchResults = competitions.Concat(countries).Concat(organisations).Concat(people).Concat(teams).Concat(venues)
                    .OrderByDescending(r => r.EffectiveTo).ThenBy(r => r.ToString()),
            };

            if (homeViewModel.SearchResults.Count() == 1)
            {
                var mainLinkData = homeViewModel.SearchResults.First().MainLinkData;

                return RedirectToAction("Index", "Details", new { area = mainLinkData.AreaType.ToString(), hk = mainLinkData.HeaderKey.ToShortGuid() });
            }

            return View(homeViewModel); 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> ByDate(DateTime? dt)
        {
            var viewModel = new MatchesByDateViewModel();

            viewModel.ViewDate = dt ?? DateTime.Today;

            viewModel.ViewDateMatchViewModels = await DbProvider.GetMatchesByDate(viewModel.ViewDate, SearchDirection.Current);
            viewModel.PreviousMatchViewModels = await DbProvider.GetMatchesByDate(viewModel.ViewDate, SearchDirection.Down);
            viewModel.NextMatchViewModels = await DbProvider.GetMatchesByDate(viewModel.ViewDate, SearchDirection.Up);

            return View(viewModel);
        }
    }
}