using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(LookupMatch lookupMatch);
        void Remove(LookupMatch lookupMatch);
        Task<IEnumerable<LookupMatch>> GetLookupMatch(Guid matchKey);
        Task<LookupMatch> GetLookupMatch(ImportSite importSite, string lookupId);
    }
}
