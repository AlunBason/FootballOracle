using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using System;

namespace FootballOracle.Models.ViewModels.Standard.Charts
{
    public class PositionDateData
    {
        public string Tooltip { get; set; }
        public CodePickerViewModel GroupData { get; set; }
        public int Position { get; set; }
        public DateTime DateValue { get; set; }
    }
}
