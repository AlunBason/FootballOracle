using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<LookupTeam> lookupTeamRepository;
        private DbSet<LookupTeam> LookupTeamRepository
        {
            get { return lookupTeamRepository = lookupTeamRepository ?? context.Set<LookupTeam>(); }
        }

        private IQueryable<LookupTeam> LookupTeams
        {
            get { return LookupTeamRepository as IQueryable<LookupTeam>; }
        }

        public void Add(LookupTeam lookupTeam)
        {
            LookupTeamRepository.Add(lookupTeam);
        }

        public void Remove(LookupTeam lookupTeam)
        {
            LookupTeamRepository.Remove(lookupTeam);
        }

        public async Task<LookupTeam> GetLookupTeam(ImportSite importSite, string lookupId)
        {
            return await LookupTeams.FirstOrDefaultAsync(s => s.ImportSite == importSite && s.LookupId == lookupId);
        }

        public async Task<LookupTeam> GetLookupTeam(Guid teamKey, ImportSite importSite)
        {
            return await LookupTeams.FirstOrDefaultAsync(f => f.TeamGuid == teamKey && f.ImportSite == importSite);
        }

        public async Task<LookupTeam> GetLookupTeamByPrimaryKey(Guid primaryKey)
        {
            return await LookupTeams.SingleOrDefaultAsync(f => f.PrimaryKey == primaryKey);
        }

        public async Task<IEnumerable<LookupTeam>> GetLookupTeamByTeamKey(Guid teamKey)
        {
            return await LookupTeams.Where(f => f.TeamGuid == teamKey).ToListAsync();
        }
    }
}
