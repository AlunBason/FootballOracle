using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.People;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(PersonV personV);
        void Remove(PersonV personV);
        Task<PersonV> GetPerson(Guid personKey, DateTime viewDate);
        Task<PersonV> GetPerson(Guid primaryKey, Guid personKey);
        Task<IEnumerable<string>> GetPersonAutoCompleteList(Guid userId, bool isAdmin, string searchText);
        Task<IEnumerable<BasePersonViewModel>> ToBasePeopleViewModels(Guid userId, bool isAdmin, DateTime viewDate, string searchText);
        Task<IEnumerable<ISearchResult>> SearchPeople(string searchText, DateTime viewDate);
    }
}
