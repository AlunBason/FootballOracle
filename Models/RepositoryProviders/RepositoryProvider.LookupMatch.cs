using FootballOracle.Foundation;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<LookupMatch> lookupMatchRepository;
        private DbSet<LookupMatch> LookupMatchRepository
        {
            get { return lookupMatchRepository = lookupMatchRepository ?? context.Set<LookupMatch>(); }
        }

        private IQueryable<LookupMatch> LookupMatches
        {
            get { return LookupMatchRepository as IQueryable<LookupMatch>; }
        }

        public async Task<IEnumerable<LookupMatch>> GetLookupMatch(Guid matchKey)
        {
            return await LookupMatches.Where(f => f.MatchGuid == matchKey).ToListAsync();
        }

        public async Task<LookupMatch> GetLookupMatch(ImportSite importSite, string lookupId)
        {
            return await LookupMatches.FirstOrDefaultAsync(f => f.ImportSite == importSite && f.LookupId == lookupId);
        }

        public void Add(LookupMatch lookupMatch)
        {
            LookupMatchRepository.Add(lookupMatch);
        }

        public void Remove(LookupMatch lookupMatch)
        {
            LookupMatchRepository.Remove(lookupMatch);
        }
    }
}
