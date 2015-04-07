using System;

namespace FootballOracle.Foundation.Interfaces
{
    public interface IApprovableEntity
    {
        Guid PrimaryKey { get; set; }
        Guid HeaderKey { get; set; }
        string WebAddress { get; set; }
        DateTime EffectiveFrom { get; set; }
        DateTime EffectiveTo { get; set; }
        bool IsActive { get; set; }
        bool IsMarkedForDeletion { get; set; }
        Guid OwnerUserId { get; set; }
        DateTime DateCreated { get; set; }
        Guid ModifiedUserId { get; set; }
        DateTime DateModified { get; set; }
    }
}
