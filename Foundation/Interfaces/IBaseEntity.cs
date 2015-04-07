using System;

namespace FootballOracle.Foundation.Interfaces
{
    public interface IBaseEntity
    {
        Guid PrimaryKey { get; set; }
    }
}
