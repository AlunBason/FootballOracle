using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class LookupMatch : BaseEntity
    {
        public ImportSite ImportSite { get; set; }

        public Guid MatchGuid { get; set; }

        [StringLength(100)]
        public string LookupId { get; set; }

        public bool HasMatchDetails { get; set; }

        #region Navigation properties
        public virtual Match Match { get; set; }
        #endregion
    }
}
