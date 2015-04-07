using System.Collections.Generic;
using FootballOracle.Models.ViewModels.Approvable.Venues;

namespace FootballOracle.Models.ViewModels.Approvable.Countries
{
    public class CountryVenuesViewModel : BaseCountryViewModel
    {
        public IEnumerable<BaseVenueViewModel> ChildVenueViewModels { get; set; }
    }
}
