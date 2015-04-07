using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(LookupVenue lookupVenue);
        void Remove(LookupVenue lookupVenue);

        Task<LookupVenue> GetLookupVenue(ImportSite importSite, string lookupId);
    }
}
