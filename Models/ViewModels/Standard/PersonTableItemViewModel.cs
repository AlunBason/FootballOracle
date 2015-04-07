using System;
using System.Collections.Generic;
using System.Linq;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.People;

namespace FootballOracle.Models.ViewModels.Standard
{
    public class PersonTableItemViewModel : IComparable<PersonTableItemViewModel>
    {
        public BasePersonViewModel PersonViewModel { get; set; }
        public int Started { get; set; }
        public int BroughtOn { get; set; }
        public int TakenOff { get; set; }
        public int Goals { get; set; }
        public int OwnGoals { get; set; }
        public int Booked { get; set; }
        public int SentOff { get; set; }

        public int CompareTo(PersonTableItemViewModel other)
        {
            if (Started != other.Started)
                return -Started.CompareTo(other.Started);

            if (BroughtOn != other.BroughtOn)
                return -BroughtOn.CompareTo(other.BroughtOn);

            return PersonViewModel.VersionEntity.Surname.CompareTo(other.PersonViewModel.VersionEntity.Surname);
        }
    }

    public static class _PersonTableItemViewModelExtensions
    {
        private static void UpdatePersonTableItemViewModels(this List<PersonTableItemViewModel> personTableItemViewModels, IGrouping<Guid, MatchEvent> matchEventGroup, DateTime viewDate)
        {
            var personViewModel = matchEventGroup.FirstOrDefault().Person.ToViewModel(viewDate);

            var query = from p in personTableItemViewModels
                        where p.PersonViewModel.PrimaryKey == personViewModel.PrimaryKey
                        select p;

            if (!query.Any())
            {
                personTableItemViewModels.Add(new PersonTableItemViewModel()
                {
                    PersonViewModel = matchEventGroup.FirstOrDefault().Person.ToViewModel(viewDate),
                    Started = matchEventGroup.Count(g => g.MatchEventType == MatchEventType.Started),
                    BroughtOn = matchEventGroup.Count(g => g.MatchEventType == MatchEventType.BroughtOn),
                    Goals = matchEventGroup.Count(g => g.MatchEventType == MatchEventType.Scored),
                    OwnGoals = matchEventGroup.Count(g => g.MatchEventType == MatchEventType.OwnGoal),
                    Booked = matchEventGroup.Count(g => g.MatchEventType == MatchEventType.Booked),
                    SentOff = matchEventGroup.Count(g => g.MatchEventType == MatchEventType.SentOff),
                });
            }
            else
            {
                var personTableItemViewModel = query.Single();
                personTableItemViewModel.Started += matchEventGroup.Count(g => g.MatchEventType == MatchEventType.Started);
                personTableItemViewModel.BroughtOn += matchEventGroup.Count(g => g.MatchEventType == MatchEventType.BroughtOn);
                personTableItemViewModel.Goals += matchEventGroup.Count(g => g.MatchEventType == MatchEventType.Scored);
                personTableItemViewModel.OwnGoals += matchEventGroup.Count(g => g.MatchEventType == MatchEventType.OwnGoal);
                personTableItemViewModel.Booked += matchEventGroup.Count(g => g.MatchEventType == MatchEventType.Booked);
                personTableItemViewModel.SentOff += matchEventGroup.Count(g => g.MatchEventType == MatchEventType.SentOff);
            }
        }
    }
}
