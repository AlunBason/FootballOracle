using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Competitions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class CompetitionV : BaseApprovableEntity
    {
        [Required]
        [StringLength(100)]
        public string CompetitionName { get; set; }

        public Guid OrganisationGuid { get; set; }

        public int CampaignPeriodValue { get; set; }
        
        public PeriodType CampaignPeriodType { get; set; }

        public int? Rank { get; set; }

        public CompetitionType? CompetitionType { get; set; }

        #region Navigation properties
        public virtual Competition Competition { get; set; }
        public virtual Organisation Organisation { get; set; }
        #endregion
    }

    public static class _CompetitionVExtensions
    {
        public static IEnumerable<BaseCompetitionViewModel> ToViewModels(this IEnumerable<CompetitionV> versions, DateTime viewDate)
        {
            foreach (var version in versions.OrderBy(o => o.Rank ?? int.MaxValue))
                yield return version.ToViewModel(viewDate);
        }

        public static BaseCompetitionViewModel ToViewModel(this CompetitionV version, DateTime viewDate)
        {
            return version.ToViewModel<BaseCompetitionViewModel, Competition, CompetitionV>(viewDate);
        }

        public static CompetitionV SetData(this CompetitionV entity, CompetitionEditorViewModel viewModel)
        {
            entity.CompetitionName = viewModel.CompetitionName;
            entity.CompetitionType = viewModel.CompetitionType;
            entity.OrganisationGuid = viewModel.OrganisationGuid;
            entity.Rank = viewModel.Rank;
            entity.WebAddress = viewModel.WebAddress;

            return entity;
        }

        public static bool GetCampaignDates(this CompetitionV competitionV, DateTime viewDate, ref DateTime startDate, ref DateTime endDate)
        {
            if (viewDate < competitionV.EffectiveFrom || viewDate > competitionV.EffectiveTo)
                return false;

            var testDate = competitionV.EffectiveFrom;

            while (testDate < competitionV.EffectiveTo)
            {
                if (viewDate >= testDate && viewDate <= testDate.AddYears(1).AddDays(-1).ToEndOfDay())
                {
                    startDate = testDate;
                    endDate = testDate.AddYears(1).AddDays(-1).ToEndOfDay();
                    return true;
                }

                testDate = testDate.AddPeriod(competitionV.CampaignPeriodType, competitionV.CampaignPeriodValue);
            }

            return false;
        }
    }
}
