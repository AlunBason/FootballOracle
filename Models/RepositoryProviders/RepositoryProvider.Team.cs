using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Team> teamRepository;
        private DbSet<Team> TeamRepository
        {
            get { return teamRepository = teamRepository ?? context.Set<Team>(); }
        }

        private IQueryable<Team> Teams
        {
            get { return TeamRepository as IQueryable<Team>; }
        }

        public void Add(Team team)
        {
            TeamRepository.Add(team);
        }

        public void Remove(Team team)
        {
            TeamRepository.Remove(team);
        }
    }
}
