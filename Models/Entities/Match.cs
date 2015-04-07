using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.Entities
{
    public class Match : BaseHeaderEntity<MatchV>
    {
        public Match()
        {
            Versions = new HashSet<MatchV>();
            LookupMatches = new HashSet<LookupMatch>();
            
        }

        public virtual ICollection<LookupMatch> LookupMatches { get; set; }
        
    }

    public static class MatchExtensions
    {
        public static IEnumerable<BaseMatchViewModel> ToViewModels(this IEnumerable<Match> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BaseMatchViewModel ToViewModel(this Match header, DateTime viewDate)
        {
            return header.ToViewModel<BaseMatchViewModel, Match, MatchV>(viewDate);
        }
    }
}
