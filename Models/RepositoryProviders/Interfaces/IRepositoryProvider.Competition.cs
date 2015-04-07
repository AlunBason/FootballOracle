using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(Competition competition);
        void Remove(Competition competition);
    }
}
