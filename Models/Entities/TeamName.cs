using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class TeamName : BaseEntity
    {
        public Guid TeamVKey { get; set; }
        public TeamNameType TeamNameType { get; set; }
        public LanguageType LanguageType { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        #region Navigation properties
        public virtual TeamV TeamV { get; set; }
        #endregion
    }
}
