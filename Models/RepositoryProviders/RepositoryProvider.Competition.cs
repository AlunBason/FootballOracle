using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Competition> competitionRepository;
        private DbSet<Competition> CompetitionRepository
        {
            get { return competitionRepository = competitionRepository ?? context.Set<Competition>(); }
        }

        private IQueryable<Competition> Competitions
        {
            get { return CompetitionRepository as IQueryable<Competition>; }
        }

        public void Add(Competition competition)
        {
            CompetitionRepository.Add(competition);
        }

        public void Remove(Competition competition)
        {
            CompetitionRepository.Remove(competition);
        }
    }
}
