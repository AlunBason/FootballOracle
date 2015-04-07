using System.Collections.Generic;

namespace FootballOracle.Foundation.Interfaces
{
    public interface IHeaderEntity<TVersion>
        where TVersion : IApprovableEntity
    {
         ICollection<TVersion> Versions { get; set; }
    }
}
