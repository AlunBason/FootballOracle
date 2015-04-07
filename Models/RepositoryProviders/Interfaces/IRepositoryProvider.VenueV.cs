using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Venues;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(VenueV venueV);
        void Remove(VenueV venueV);
        Task<VenueV> GetVenue(Guid venueKey, DateTime viewDate);
        Task<VenueV> GetVenue(Guid primaryKey, Guid venueKey);
        Task<IEnumerable<string>> GetVenueAutoCompleteList(Guid userId, bool isAdmin, string searchText);
        Task<IEnumerable<BaseVenueViewModel>> GetCountryVenueViewModels(Guid countryHeaderKey, DateTime viewDate);
        Task<IEnumerable<ICodePickerData>> GetVenueCodePickerData(DateTime viewDate);
        Task<IEnumerable<ICodePickerData>> GetCascadeVenueCodePickerData(DateTime viewDate, Guid countryKey);
        Task<IEnumerable<ISearchResult>> SearchVenues(string searchText, DateTime viewDate);
    }
}
