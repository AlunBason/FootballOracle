using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<LookupPerson> lookupPersonRepository;
        private DbSet<LookupPerson> LookupPersonRepository
        {
            get { return lookupPersonRepository = lookupPersonRepository ?? context.Set<LookupPerson>(); }
        }

        private IQueryable<LookupPerson> LookupPeople
        {
            get { return LookupPersonRepository as IQueryable<LookupPerson>; }
        }

        public void Add(LookupPerson lookupPerson)
        {
            LookupPersonRepository.Add(lookupPerson);
        }

        public void Remove(LookupPerson lookupPerson)
        {
            LookupPersonRepository.Remove(lookupPerson);
        }

        public async Task<LookupPerson> GetLookupPerson(ImportSite importSite, string lookupId)
        {
            return await LookupPeople.FirstOrDefaultAsync(s => s.ImportSite == importSite && s.LookupId == lookupId);
        }
    }
}
