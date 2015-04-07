using System;
using FootballOracle.Foundation;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchPersonViewModel : IComparable<MatchPersonViewModel>
    {
        public Guid PersonGuid { get; set; }
        public string PersonName { get; set; }
        public PositionType? PositionType { get; set; }
        public MatchEventStartType? MatchEventStartType { get; set; }

        public int CompareTo(MatchPersonViewModel other)
        {
            if (MatchEventStartType == null)
                return 1;

            if (other.MatchEventStartType == null)
                return -1;

            if (MatchEventStartType != other.MatchEventStartType)
                return MatchEventStartType > other.MatchEventStartType ? 1 : -1;

            if (PositionType != other.PositionType)
                return PositionType > other.PositionType ? 1 : -1;

            return PersonName.CompareTo(other.PersonName);
        }
    }
}
