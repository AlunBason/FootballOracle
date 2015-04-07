using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class LookupTeam : BaseEntity
    {
        public ImportSite ImportSite { get; set; }

        public Guid TeamGuid { get; set; }

        [StringLength(100)]
        public string LookupId { get; set; }

        #region Navigation properties
        public virtual Team Team { get; set; }
        #endregion
    }
}
