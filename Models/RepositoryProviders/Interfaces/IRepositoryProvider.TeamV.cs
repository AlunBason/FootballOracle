using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(TeamV teamV);
        void Remove(TeamV teamV);
        void Attach(TeamV teamV);
        Task<TeamV> GetTeam(Guid teamKey, DateTime viewDate);
        Task<TeamV> GetTeam(Guid primaryKey, Guid teamKey);
        Task<IEnumerable<string>> GetTeamAutoCompleteList(Guid userId, bool isAdmin, string searchText);
        Task<IEnumerable<ICodePickerData>> GetTeamCodePickerData(DateTime viewDate);
        Task<IEnumerable<BaseTeamViewModel>> GetCountryTeamViewModels(Guid countryHeaderKey, DateTime viewDate);
        Task<IEnumerable<ISearchResult>> SearchTeams(string searchtext, DateTime viewDate);
        Task SyncTeamNames();
    }
}
