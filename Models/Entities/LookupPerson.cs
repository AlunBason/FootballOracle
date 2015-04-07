using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class LookupPerson : BaseEntity
    {
        public ImportSite ImportSite { get; set; }

        public Guid PersonGuid { get; set; }

        [StringLength(100)]
        public string LookupId { get; set; }

        #region Navigation properties
        public virtual Person Person { get; set; }
        #endregion
    }
}
