using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Organisations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(OrganisationV organisationV);
        void Remove(OrganisationV organisationV);
        Task<OrganisationV> GetOrganisation(Guid organisationKey, DateTime viewDate);
        Task<OrganisationV> GetOrganisation(Guid primaryKey, Guid organisationKey);
        Task<IEnumerable<string>> GetOrganisationAutoCompleteList(Guid userId, bool isAdmin, string searchText);
        Task<IEnumerable<BaseOrganisationViewModel>> GetOrganisationsByCountry(Guid countryHeaderKey, DateTime viewDate);
        Task<IEnumerable<BaseOrganisationViewModel>> GetChildOrganisations(Guid organisationHeaderKey, DateTime viewDate);
        Task<IEnumerable<ICodePickerData>> GetOrganisationCodePickerData(DateTime viewDate);
        Task<IEnumerable<ISearchResult>> SearchOrganisations(string searchText, DateTime viewDate);
    }
}
