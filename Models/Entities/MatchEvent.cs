using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Models.ViewModels.Standard;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.Entities
{
    public class MatchEvent : BaseEntity
    {
        public Guid MatchVPrimaryKey { get; set; }

        public Guid TeamPrimaryKey { get; set; }

        public Guid PersonPrimaryKey { get; set; }

        public PositionType? PositionType { get; set; }

        public MatchEventType MatchEventType { get; set; }

        public short? Minute { get; set; }

        public short? Extra { get; set; }

        #region Navigation properties
        public virtual MatchV MatchV { get; set; }
        public virtual Person Person { get; set; }
        public virtual Team Team { get; set; }
        #endregion
    }

    public static class MatchEventExtensions
    {
        public static IEnumerable<MatchEventViewModel> ToViewModels(this IEnumerable<MatchEvent> matchEvents, DateTime viewDate)
        {
            foreach (var matchEvent in matchEvents)
                yield return matchEvent.ToViewModel(viewDate);
        }

        public static MatchEventViewModel ToViewModel(this MatchEvent matchEvent, DateTime viewDate)
        {
            return new MatchEventViewModel()
            {
                Entity = matchEvent,
                ViewDate = viewDate
            };
        }

        
    }
}
