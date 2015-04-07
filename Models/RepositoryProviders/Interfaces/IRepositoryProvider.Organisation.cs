using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(Organisation organisation);
        void Remove(Organisation organisation);
    }
}
