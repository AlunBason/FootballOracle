using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders.Interfaces
{
    public partial interface IRepositoryProvider
    {
        void Add(TeamName teamName);
        void Remove(TeamName teamName);
        void Attach(TeamName teamName);
    }
}
