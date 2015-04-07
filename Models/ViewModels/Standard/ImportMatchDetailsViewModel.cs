using System;
using System.ComponentModel.DataAnnotations;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class ImportMatchDetailsViewModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
