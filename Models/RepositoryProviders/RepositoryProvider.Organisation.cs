using FootballOracle.Models.Entities;
using System.Data.Entity;
using System.Linq;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Organisation> organisationRepository;
        private DbSet<Organisation> OrganisationRepository
        {
            get { return organisationRepository = organisationRepository ?? context.Set<Organisation>(); }
        }

        private IQueryable<Organisation> Organisations
        {
            get { return OrganisationRepository as IQueryable<Organisation>; }
        }

        public void Add(Organisation organisation)
        {
            OrganisationRepository.Add(organisation);
        }

        public void Remove(Organisation organisation)
        {
            OrganisationRepository.Remove(organisation);
        }
    }
}
