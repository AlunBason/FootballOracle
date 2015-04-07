using System.Data.Entity;
using System.Linq;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(Venue venue);
        void Remove(Venue venue);
    }
}
