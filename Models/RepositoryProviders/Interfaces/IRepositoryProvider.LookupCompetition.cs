using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballOracle.Foundation;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(LookupCompetition lookupCompetition);
        void Remove(LookupCompetition lookupCompetition);

        Task<LookupCompetition> GetLookupCompetition(ImportSite importSite, string lookupId);
        Task<LookupCompetition> GetLookupCompetition(Guid competitionKey, ImportSite importSite);
        Task<IEnumerable<LookupCompetition>> GetLookupCompetitionByCompetitionKey(Guid competitionKey);
        Task<LookupCompetition> GetLookupCompetitionByPrimaryKey(Guid primaryKey);
    }
}
