using System;

namespace FootballOracle.Foundation.Interfaces
{
    public interface IApprovableLinkData
    {
        AreaType AreaType { get; }
        Guid HeaderKey { get; }
        DateTime ViewDate { get; }
    }
}
