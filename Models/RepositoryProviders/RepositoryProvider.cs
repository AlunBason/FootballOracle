using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using FootballOracle.Models.DbContexts.Pf;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders.Interfaces;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider : IRepositoryProvider, IDisposable
    {
        private PfDbContext context = new PfDbContext();

        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());

                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb.ToString(), ex); // Add the original exception as the innerException
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
