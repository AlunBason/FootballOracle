using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.Entities
{
    public class Resource
    {
        [Key]
        public Guid PrimaryKey { get; set; }

        public byte[] ResourceBytes { get; set; }

        #region Navigation properties
        public virtual ICollection<TeamV> TeamVs { get; set; }
        public virtual ICollection<CountryV> CountryVs { get; set; }
        #endregion
    }
}
