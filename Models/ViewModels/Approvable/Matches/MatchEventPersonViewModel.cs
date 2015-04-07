using System;
using FootballOracle.Foundation;
using FootballOracle.Models.Attributes;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchEventPersonViewModel : IComparable<MatchEventPersonViewModel>
    {
        public Guid? PrimaryKey { get; set; }
        public Guid? PersonPrimaryKey { get; set; }
        public string PersonName { get; set; }
        public MatchEventInRunningType? MatchEventInRunningType { get; set; }

        [HideLabel]
        public short? Minute { get; set; }

        [HideLabel]
        public short? Extra { get; set; }

        public int CompareTo(MatchEventPersonViewModel other)
        {
            if (Minute != other.Minute)
                return Minute > other.Minute ? 1 : -1;

            return Extra > other.Extra ? 1 : -1;
        }
    }
}
