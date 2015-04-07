using FootballOracle.Models.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Match> matchRepository;
        private DbSet<Match> MatchRepository
        {
            get { return matchRepository = matchRepository ?? context.Set<Match>(); }
        }

        private IQueryable<Match> Matches
        {
            get { return MatchRepository as IQueryable<Match>; }
        }

        public void Add(Match match)
        {
            MatchRepository.Add(match);
        }

        public void Remove(Match match)
        {
            MatchRepository.Remove(match);
        }

        public async Task<Match> GetMatch(Guid matchKey)
        {
            return await Matches.SingleAsync(s => s.PrimaryKey == matchKey);
        }
    }
}
