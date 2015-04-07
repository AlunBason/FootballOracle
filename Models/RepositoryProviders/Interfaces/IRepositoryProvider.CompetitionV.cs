using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(CompetitionV competitionV);
        void Remove(CompetitionV competitionV);
        Task<CompetitionV> GetCompetition(Guid competitionKey, DateTime viewDate);
        Task<CompetitionV> GetCompetition(Guid primaryKey, Guid competitionKey);
        Task<IEnumerable<string>> GetCompetitionAutoCompleteListAsync(Guid userId, bool isAdmin, string searchText);
        Task<IEnumerable<BaseCompetitionViewModel>> GetCompetitionsByOrganisationAsync(Guid organisationHeaderKey, DateTime viewDate);
        Task<IEnumerable<ISearchResult>> SearchCompetitions(string searchText, DateTime viewDate);
    }
}
