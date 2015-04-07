using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;
using FootballOracle.Foundation;
using System.Collections.Generic;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<LookupCompetition> lookupCompetitionRepository;
        private DbSet<LookupCompetition> LookupCompetitionRepository
        {
            get { return lookupCompetitionRepository = lookupCompetitionRepository ?? context.Set<LookupCompetition>(); }
        }

        private IQueryable<LookupCompetition> LookupCompetitions
        {
            get { return LookupCompetitionRepository as IQueryable<LookupCompetition>; }
        }

        public void Add(LookupCompetition lookupCompetition)
        {
            LookupCompetitionRepository.Add(lookupCompetition);
        }

        public void Remove(LookupCompetition lookupCompetition)
        {
            LookupCompetitionRepository.Remove(lookupCompetition);
        }

        public async Task<LookupCompetition> GetLookupCompetition(ImportSite importSite, string lookupId)
        {
            return await LookupCompetitions.FirstOrDefaultAsync(l => l.ImportSite == importSite && l.LookupId == lookupId);
        }

        public async Task<LookupCompetition> GetLookupCompetition(Guid competitionKey, ImportSite importSite)
        {
            return await LookupCompetitions.FirstOrDefaultAsync(l => l.CompetitionGuid == competitionKey && l.ImportSite == importSite);
        }

        public async Task<IEnumerable<LookupCompetition>> GetLookupCompetitionByCompetitionKey(Guid competitionKey)
        {
            return await LookupCompetitions.Where(l => l.CompetitionGuid == competitionKey).ToListAsync();
        }

        public async Task<LookupCompetition> GetLookupCompetitionByPrimaryKey(Guid primaryKey)
        {
            return await LookupCompetitions.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey);
        }
    }
}
