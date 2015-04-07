using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Person> personRepository;
        private DbSet<Person> PersonRepository
        {
            get { return personRepository = personRepository ?? context.Set<Person>(); }
        }

        private IQueryable<Person> People
        {
            get { return PersonRepository as IQueryable<Person>; }
        }

        public void Add(Person person)
        {
            PersonRepository.Add(person);
        }

        public void Remove(Person person)
        {
            PersonRepository.Remove(person);
        }
    }
}
