using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Countries;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Models.ViewModels.Base;
using FootballOracle.Models.ViewModels.Standard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Approvable.People
{
    public class BasePersonViewModel : BaseApprovableViewModel<Person, PersonV>, ISearchResult, IApprovableLinkData
    {
        public override string ToString()
        {
            if (string.IsNullOrEmpty(VersionEntity.Forenames))
                return VersionEntity.Surname;
            else
            {
                return string.Format("{0} {1}", VersionEntity.Forenames, VersionEntity.Surname);
            }
        }

        private BaseTeamViewModel teamViewModel;
        public BaseTeamViewModel TeamViewModel
        {
            get { return teamViewModel = teamViewModel ?? this.TeamViewModel(ViewDate); }
        }

        private PersonTableItemViewModel personTableItemViewModel;
        public PersonTableItemViewModel PersonTableItemViewModel
        {
            get
            {
                return personTableItemViewModel = personTableItemViewModel ?? new PersonTableItemViewModel()
                {
                    PersonViewModel = this,
                    Started = VersionEntity.Person.MatchEvents.Count(m => m.MatchEventType == MatchEventType.Started),
                    BroughtOn = VersionEntity.Person.MatchEvents.Count(g => g.MatchEventType == MatchEventType.BroughtOn),
                    Goals = VersionEntity.Person.MatchEvents.Count(g => g.MatchEventType == MatchEventType.Scored),
                    OwnGoals = VersionEntity.Person.MatchEvents.Count(g => g.MatchEventType == MatchEventType.OwnGoal),
                    Booked = VersionEntity.Person.MatchEvents.Count(g => g.MatchEventType == MatchEventType.Booked),
                    SentOff = VersionEntity.Person.MatchEvents.Count(g => g.MatchEventType == MatchEventType.SentOff)
                };
            }
        }

        private IEnumerable<BaseMatchViewModel> matchViewModels;
        public IEnumerable<BaseMatchViewModel> MatchViewModels
        {
            get
            {
                if (matchViewModels != null)
                    return matchViewModels;

                var matchList = new List<BaseMatchViewModel>();

                foreach (var item in VersionEntity.Person.MatchEvents)
                    if (matchList.Any(m => m.PrimaryKey != item.PrimaryKey))
                        matchList.Add(item.MatchV.ToViewModel(ViewDate));

                return matchViewModels = matchList;
            }
        }

        public BaseCountryViewModel CountryViewModel
        {
            get { return VersionEntity.Country.GetApprovedVersion<CountryV>(ViewDate).ToViewModel(ViewDate); }
        }


        public IApprovableLinkData ParentLinkData
        {
            get 
            {
                if (VersionEntity.Country != null)
                    return CountryViewModel;

                return null;
            }
        }

        public override IApprovableLinkData ParentLinkData2
        {
            get { return TeamViewModel; }
        }

        public AreaType AreaType
        {
            get { return AreaType.Ppl; }
        }

        public override byte[] ResourceBytes
        {
            get 
            {
                if (TeamViewModel == null)
                    return null;

                return TeamViewModel.VersionEntity.Resource != null ? teamViewModel.VersionEntity.Resource.ResourceBytes : null;
            }
        }

        public override Person HeaderEntity
        {
            get { return VersionEntity.Person; }
        }
    }

    public static class _PersonViewModelExtensions
    {
        public static BaseTeamViewModel TeamViewModel(this BasePersonViewModel personViewModel, DateTime viewDate)
        {
            var lastMatchEvent = personViewModel.VersionEntity.Person.MatchEvents.OrderByDescending(m => m.MatchV.MatchDate).FirstOrDefault(m => m.MatchV.MatchDate <= viewDate);

            if (lastMatchEvent == null)
                return null;

            return lastMatchEvent.Team.ToViewModel(viewDate);
        }
    }
}