using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class LookupVenue : BaseEntity
    {
        public ImportSite ImportSite { get; set; }

        public Guid VenueGuid { get; set; }

        [StringLength(100)]
        public string LookupId { get; set; }

        #region Navigation properties
        public virtual Venue Venue { get; set; }
        #endregion
    }
}
