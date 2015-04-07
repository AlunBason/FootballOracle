using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(LookupTeam lookupTeam);
        void Remove(LookupTeam lookupTeam);

        Task<LookupTeam> GetLookupTeam(ImportSite importSite, string lookupId);
        Task<LookupTeam> GetLookupTeam(Guid teamKey, ImportSite importSite);
        Task<LookupTeam> GetLookupTeamByPrimaryKey(Guid primaryKey);
        Task<IEnumerable<LookupTeam>> GetLookupTeamByTeamKey(Guid teamKey);
    }
}
