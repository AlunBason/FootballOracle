using System;
using System.Collections.Generic;
using FootballOracle.Foundation.Interfaces;

namespace FootballOracle.Foundation.Entities
{
    public abstract class BaseHeaderEntity<TVersion> : BaseEntity, IHeaderEntity<TVersion>
        where TVersion : IApprovableEntity
    {
        public virtual ICollection<TVersion> Versions { get; set; }

        public static THeader Create<THeader>()
            where THeader : BaseEntity, new()
        {
            return new THeader() { PrimaryKey = Guid.NewGuid() };
        }
    }
}
