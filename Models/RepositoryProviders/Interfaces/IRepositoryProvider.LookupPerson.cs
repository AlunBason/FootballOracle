using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(LookupPerson lookupPerson);
        void Remove(LookupPerson lookupPerson);

        Task<LookupPerson> GetLookupPerson(ImportSite importSite, string lookupId);
    }
}
