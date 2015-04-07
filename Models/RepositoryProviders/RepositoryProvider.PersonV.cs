using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.People;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<PersonV> personVRepository;
        private DbSet<PersonV> PersonVRepository
        {
            get { return personVRepository = personVRepository ?? context.Set<PersonV>(); }
        }

        private IQueryable<PersonV> PersonVs
        {
            get { return PersonVRepository as IQueryable<PersonV>; }
        }

        public void Add(PersonV personV)
        {
            PersonVRepository.Add(personV);
        }

        public void Remove(PersonV personV)
        {
            PersonVRepository.Remove(personV);
        }

        public async Task<PersonV> GetPerson(Guid personKey, DateTime viewDate)
        {
            return (await People.SingleOrDefaultAsync(f => f.PrimaryKey == personKey)).GetApprovedVersion(viewDate);
        }

        public async Task<PersonV> GetPerson(Guid primaryKey, Guid personKey)
        {
            return await PersonVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == personKey);
        }

        public async Task<IEnumerable<string>> GetPersonAutoCompleteList(Guid userId, bool isAdmin, string searchText)
        {
            var people = await PersonVs.Where(w => w.SearchText.Contains(searchText.Trim()) && w.IsActive).ToListAsync();

            return people.Select(s => string.Format("{0} {1}", s.Forenames, s.Surname).Trim()).Distinct().OrderBy(o => o);
        }

        public async Task<IEnumerable<BasePersonViewModel>> ToBasePeopleViewModels(Guid userId, bool isAdmin, DateTime viewDate, string searchText)
        {
            var people = await PersonVs.Where(w => w.SearchText.Contains(searchText.Trim()) && w.IsActive).ToListAsync();

            return people.ToViewModels(viewDate);
        }

        public async Task<IEnumerable<ISearchResult>> SearchPeople(string searchText, DateTime viewDate)
        {
            var normalizedText = searchText.RemoveDiacritics();

            var groups = await PersonVs
                .Where(t => (t.SearchText.Contains(normalizedText.Trim()) || t.SearchText.Contains(searchText.Trim())) && t.IsActive)
                .GroupBy(t => t.HeaderKey).ToListAsync();

            var versions = new List<PersonV>();

            foreach (var group in groups)
                versions.Add(group.OrderByDescending(t => t.EffectiveFrom).First());

            return versions.ToViewModels(viewDate).Cast<ISearchResult>();
        }
    }
}
