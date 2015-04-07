using FootballOracle.Foundation;
using FootballOracle.Models.Entities;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class PersonMatchEventCountViewModel
    {
        public Person Person { get; set; }
        public MatchEventType MatchEventType { get; set; }
        public int Count { get; set; }
    }
}
