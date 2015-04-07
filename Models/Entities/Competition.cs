using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class Competition : BaseHeaderEntity<CompetitionV>
    {
        public Competition()
        {
            Campaigns = new HashSet<Campaign>();
            LookupCompetitions = new HashSet<LookupCompetition>();
            Versions = new HashSet<CompetitionV>();
        }

        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<LookupCompetition> LookupCompetitions { get; set; }
    }

    public static class CompetitionExtensions
    {
        public static IEnumerable<BaseCompetitionViewModel> ToViewModels(this IEnumerable<Competition> headers, DateTime viewDate)
        {
            foreach (var header in headers)
                yield return header.ToViewModel(viewDate);
        }

        public static BaseCompetitionViewModel ToViewModel(this Competition header, DateTime viewDate)
        {
            return header.ToViewModel<BaseCompetitionViewModel, Competition, CompetitionV>(viewDate);
        }

        public static IEnumerable<BaseCompetitionViewModel> GetEditableVersions(this Competition entity, DateTime viewDate)
        {
            return entity.Versions.OrderByDescending(v => v.EffectiveFrom).ThenByDescending(v => v.IsActive).ToViewModels(viewDate);
        }

        public static CountryV GetParentCountry(this Competition competition, DateTime viewDate)
        {
            var competitionV = competition.GetApprovedVersion<CompetitionV>(viewDate);

            if (competitionV == null)
                return null;

            if (competitionV.OrganisationGuid != null)
                return competitionV.Organisation.GetParentCountry(viewDate);

            return null;
        }

        public static IEnumerable<DateRangePickerViewModel> ToDateRangePickerViewModels(this Competition competition)
        {
            var dateRangePickerViewModels = new List<DateRangePickerViewModel>();
            
            foreach (var campaign in competition.Campaigns.OrderByDescending(o => o.EndDate))
            {
                dateRangePickerViewModels.Add(new DateRangePickerViewModel()
                {
                    EndDateString = campaign.EndDate.ToUrlString(),
                    Description = string.Format("{0}/{1}", campaign.StartDate.Year, campaign.EndDate.Year)
                });
            }
            return dateRangePickerViewModels;
        }
    }
}
