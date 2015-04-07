using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Matches;
using System;
using System.Collections.Generic;

namespace FootballOracle.Models.Entities
{
    public class MatchV : BaseApprovableEntity
    {
        public MatchV()
        {
            MatchEvents = new HashSet<MatchEvent>();
        }

        public MatchImportType MatchImportType { get; set; }

        public DateTime MatchDate { get; set; }

        public long? MatchTimeTicks { get; set; }

        public Guid CampaignStageKey { get; set; }

        public Guid? VenueGuid { get; set; }

        public int? Attendance { get; set; }

        public Guid Team1Guid { get; set; }

        public short? Team1HT { get; set; }

        public short? Team1FT { get; set; }

        public Guid Team2Guid { get; set; }

        public short? Team2HT { get; set; }

        public short? Team2FT { get; set; }

        #region Navigation properties
        public virtual Match Match { get; set; }
        public virtual CampaignStage CampaignStage { get; set; }
        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
        #endregion
    }

    public static class _MatchVExtensions
    {
        public static IEnumerable<BaseMatchViewModel> ToViewModels(this IEnumerable<MatchV> versions, DateTime viewDate)
        {
            foreach (var version in versions)
                yield return version.ToViewModel(viewDate);
        }

        public static BaseMatchViewModel ToViewModel(this MatchV version, DateTime viewDate)
        {
            return version.ToViewModel<BaseMatchViewModel, Match, MatchV>(viewDate);
        }

        public static void SetData(this MatchV entityV, MatchEditorViewModel viewModel)
        {
            entityV.MatchImportType = entityV.GetMatchImportType(false);
            entityV.MatchDate = viewModel.MatchDate.Date;
            entityV.MatchTimeTicks = (viewModel.MatchDate - viewModel.MatchDate.Date).Ticks;
            entityV.CampaignStageKey = viewModel.CampaignStageViewModel.Entity.PrimaryKey;
            entityV.VenueGuid = viewModel.VenueGuid;
            entityV.Attendance = viewModel.Attendance;
            entityV.Team1Guid = viewModel.Team1Guid;
            entityV.Team1HT = viewModel.Team1Ht;
            entityV.Team1FT = viewModel.Team1Ft;
            entityV.Team2Guid = viewModel.Team2Guid;
            entityV.Team2HT = viewModel.Team2Ht;
            entityV.Team2FT = viewModel.Team2Ft;
        }

        public static void SetData(this MatchV entityV, MatchV source)
        {
            entityV.MatchImportType = entityV.GetMatchImportType(false);
            entityV.MatchDate = source.MatchDate;
            entityV.MatchTimeTicks = source.MatchTimeTicks;
            entityV.CampaignStageKey = source.CampaignStageKey;
            entityV.VenueGuid = source.VenueGuid;
            entityV.Attendance = source.Attendance;
            entityV.Team1Guid = source.Team1Guid;
            entityV.Team1HT = source.Team1HT;
            entityV.Team1FT = source.Team1FT;
            entityV.Team2Guid = source.Team2Guid;
            entityV.Team2HT = source.Team2HT;
            entityV.Team2FT = source.Team2FT;
        }

        public static void ToMatchEditorViewModel(this MatchV entity, MatchEditorViewModel viewModel)
        {
            viewModel.MatchDate = viewModel.VersionEntity.MatchDate.AddTicks((long)viewModel.VersionEntity.MatchTimeTicks);
            viewModel.CompetitionGuid = viewModel.VersionEntity.CampaignStage.Campaign.CompetitionKey;
            viewModel.VenueGuid = viewModel.VersionEntity.VenueGuid;
            viewModel.Attendance = viewModel.VersionEntity.Attendance;
            viewModel.Team1Guid = viewModel.VersionEntity.Team1Guid;
            viewModel.Team1Ht = viewModel.VersionEntity.Team1HT;
            viewModel.Team1Ft = viewModel.VersionEntity.Team1FT;
            viewModel.Team2Guid = viewModel.VersionEntity.Team2Guid;
            viewModel.Team2Ht = viewModel.VersionEntity.Team2HT;
            viewModel.Team2Ft = viewModel.VersionEntity.Team2FT;
        }

        public static MatchImportType GetMatchImportType (this MatchV matchV, bool isAutomatic)
        {
            if (matchV.Team1FT != null && matchV.Team2FT != null)
                return isAutomatic ? MatchImportType.AutomaticResultOnly : MatchImportType.ManualResult;
               
            return isAutomatic ? MatchImportType.AutomaticFixture : MatchImportType.ManualFixture;
        }

        
    }
}
