using System.Data.Entity;
using System.Linq;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<TeamName> teamNameRepository;
        private DbSet<TeamName> TeamNameRepository
        {
            get { return teamNameRepository = teamNameRepository ?? context.Set<TeamName>(); }
        }

        private IQueryable<TeamName> TeamNames
        {
            get { return TeamNameRepository as IQueryable<TeamName>; }
        }

        public void Add(TeamName teamName)
        {
            TeamNameRepository.Add(teamName);
        }

        public void Remove(TeamName teamName)
        {
            TeamNameRepository.Remove(teamName);
        }

        public void Attach(TeamName teamName)
        {
            context.TeamNames.Attach(teamName);
            context.Entry(teamName).State = EntityState.Modified;
        }
    }
}
