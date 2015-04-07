using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.People;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.Entities
{
    public class Person : BaseHeaderEntity<PersonV>
    {
        public Person()
        {
            Versions = new HashSet<PersonV>();
            MatchEvents = new HashSet<MatchEvent>();
            LookupPeople = new HashSet<LookupPerson>();
        }

        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
        public virtual ICollection<LookupPerson> LookupPeople { get; set; }
    }

    public static class PersonExtensions
    {
        public static IEnumerable<BasePersonViewModel> ToViewModels(this IEnumerable<Person> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BasePersonViewModel ToViewModel(this Person header, DateTime viewDate)
        {
            return header.ToViewModel<BasePersonViewModel, Person, PersonV>(viewDate);
        }
    }
}
