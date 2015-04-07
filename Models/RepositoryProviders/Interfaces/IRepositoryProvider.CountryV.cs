using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(CountryV countryV);
        void Remove(CountryV countryV);
        Task<CountryV> GetCountry(Guid countryKey, DateTime viewDate);
        Task<CountryV> GetCountry(Guid primaryKey, Guid countryKey);
        Task<IEnumerable<string>> GetCountryAutoCompleteListAsync(Guid userId, bool isAdmin, string searchText);
        Task<IEnumerable<BaseCountryViewModel>> GetCountriesByOrganisationAsync(Guid organisationHeaderKey, DateTime viewDate);
        Task<IEnumerable<ICodePickerData>> GetCountryCodePickerData(DateTime viewDate);
        Task<IEnumerable<ISearchResult>> SearchCountries(string searchText, DateTime viewDate);

    }
}
