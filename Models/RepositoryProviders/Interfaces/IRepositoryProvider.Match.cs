using FootballOracle.Models.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(Match match);
        void Remove(Match match);

        Task<Match> GetMatch(Guid matchKey);
    }
}
