using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using FootballOracle.Foundation.Interfaces;

namespace FootballOracle.Foundation.Entities
{
    public abstract class BaseApprovableEntity : BaseEntity, IApprovableEntity
    {
        [Column(Order = 1)]
        [ScaffoldColumn(false)]
        public Guid HeaderKey { get; set; }

        public Guid? ResourceGuid { get; set; }

        public string WebAddress { get; set; }

        public virtual DateTime EffectiveFrom { get; set; }

        public virtual DateTime EffectiveTo { get; set; }

        [ScaffoldColumn(false)]
        public bool IsMarkedForDeletion { get; set; }

        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }

        [ScaffoldColumn(false)]
        public Guid OwnerUserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DateCreated { get; set; }

        [ScaffoldColumn(false)]
        public Guid ModifiedUserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DateModified { get; set; }

        #region Static methods
        public static TEntity CreateNew<TEntity>(Guid ownerUserId)
            where TEntity : IApprovableEntity, new()
        {
            return new TEntity()
            {
                PrimaryKey = Guid.NewGuid(),
                IsMarkedForDeletion = false,
                IsActive = true,
                OwnerUserId = ownerUserId,
                ModifiedUserId = ownerUserId,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
        }

        public static TEntity CreateNewVersion<TEntity>(Guid ownerUserId, Guid modifiedUserId)
            where TEntity : IApprovableEntity, new()
        {
            var newEntity = CreateNew<TEntity>(ownerUserId);
            newEntity.ModifiedUserId = modifiedUserId;

            return newEntity;
        }        
        #endregion
    }
}
