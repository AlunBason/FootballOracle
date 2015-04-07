using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchHeadToHeadViewModel : BaseMatchViewModel
    {
        public IEnumerable<BaseMatchViewModel> HeadToHeadMatchViewModels { get; set; }
    }
}
