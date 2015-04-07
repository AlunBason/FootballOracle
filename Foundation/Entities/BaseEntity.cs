using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FootballOracle.Foundation.Interfaces;

namespace FootballOracle.Foundation.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        [Column(Order = 0)]
        [ScaffoldColumn(false)]
        public Guid PrimaryKey { get; set; }
    }
}
