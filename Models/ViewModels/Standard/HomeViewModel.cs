using System.Collections.Generic;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            SearchResults = new List<BaseCountryViewModel>();
        }

        public IEnumerable<ISearchResult> SearchResults { get; set; }
    }
}
