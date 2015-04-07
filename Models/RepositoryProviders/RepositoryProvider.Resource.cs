using System.Data.Entity;
using System.Linq;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<Resource> resourceRepository;
        private DbSet<Resource> ResourceRepository
        {
            get { return resourceRepository = resourceRepository ?? context.Set<Resource>(); }
        }

        private IQueryable<Resource> Resources
        {
            get { return ResourceRepository as IQueryable<Resource>; }
        }

        public void Add(Resource resource)
        {
            ResourceRepository.Add(resource);
        }

        public void Remove(Resource resource)
        {
            ResourceRepository.Remove(resource);
        }
    }
}
