using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Models.Entities;
using FootballOracle.Models.RepositoryProviders;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FootballOracle.Models.ViewModels.Approvable.Matches
{
    public class MatchEditorViewModel : BaseMatchViewModel, IEquatable<MatchV>
    {
        #region EditorProperties
        [Display(Name = "Match date")]
        [DataType(DataType.DateTime)]
        public override DateTime MatchDate { get; set; }

        [Display(Name = "Competition")]
        public override Guid? CompetitionGuid { get; set; }

        [UIHint("DropDowns/Venue")]
        [Display(Name = "Venue")]
        public override Guid? VenueGuid { get; set; }

        public int? Attendance { get; set; }

        [Display(Name = "Team 1")]
        public override Guid Team1Guid { get; set; }

        [Display(Name = "Goals (Half-time)")]
        public override short? Team1Ht { get; set; }

        [Display(Name = "Goals (Full-time)")]
        public override short? Team1Ft { get; set; }

        [Display(Name = "Team 2")]
        public override Guid Team2Guid { get; set; }

        [Display(Name = "Goals (Half-time)")]
        public override short? Team2Ht { get; set; }

        [Display(Name = "Goals (Full-time)")]
        public override short? Team2Ft { get; set; }
        #endregion

        public IEnumerable<BaseMatchViewModel> MatchViewModels { get; set; }

        public void GetEntityData()
        {
            MatchDate = VersionEntity.MatchDate.AddTicks((long)VersionEntity.MatchTimeTicks);
            CompetitionGuid = VersionEntity.CampaignStage.Campaign.CompetitionKey;
            VenueGuid = VersionEntity.VenueGuid;
            Attendance = VersionEntity.Attendance;
            Team1Guid = VersionEntity.Team1Guid;
            Team1Ht = VersionEntity.Team1HT;
            Team1Ft = VersionEntity.Team1FT;
            Team2Guid = VersionEntity.Team2Guid;
            Team2Ht = VersionEntity.Team2HT;
            Team2Ft = VersionEntity.Team2FT;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public IEnumerable<ICodePickerData> CompetitionPickerData { get; set; }

        public string CompetitionName { get; set; }
        public string VenueName { get; set; }

        public void SetVenueName(IRepositoryProvider provider)
        {
            VenueName = VenueGuid != null ? provider.GetVenue((Guid)VenueGuid, MatchDate).ToString() : string.Empty;
        }

        public IEnumerable<ICodePickerData> TeamPickerData { get; set; }

        public async Task SetTeamPickerData(IRepositoryProvider provider)
        {
            TeamPickerData = await provider.GetTeamCodePickerData(DateTime.Now);
        }

        public bool Equals(MatchV other)
        {
            return MatchDate.Date == other.MatchDate
                && (MatchDate - MatchDate.Date).Ticks == other.MatchTimeTicks
                && CompetitionGuid == other.CampaignStage.Campaign.CompetitionKey
                && VenueGuid == other.VenueGuid
                && Attendance == other.Attendance
                && Team1Guid == other.Team1Guid
                && Team1Ht == other.Team1HT
                && Team1Ft == other.Team1FT
                && Team2Guid == other.Team2Guid
                && Team2Ht == other.Team2HT
                && Team2Ft == other.Team2FT;
        }
    }

    public static class MatchEditorViewModelExtensions
    {
        public static MatchV ToMatchV(this MatchEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId, Guid campaignStageGuid)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<MatchV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            newEntityV.MatchDate = editorViewModel.MatchDate.Date;
            newEntityV.MatchTimeTicks = (editorViewModel.MatchDate - editorViewModel.MatchDate.Date).Ticks;
            newEntityV.CampaignStageKey = campaignStageGuid;
            newEntityV.VenueGuid = editorViewModel.VenueGuid;
            newEntityV.Attendance = editorViewModel.Attendance;
            newEntityV.Team1Guid = editorViewModel.Team1Guid;
            newEntityV.Team1HT = editorViewModel.Team1Ht;
            newEntityV.Team1FT = editorViewModel.Team1Ft;
            newEntityV.Team2Guid = editorViewModel.Team2Guid;
            newEntityV.Team2HT = editorViewModel.Team2Ht;
            newEntityV.Team2FT = editorViewModel.Team2Ft;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = Date.LowDate;
            newEntityV.EffectiveTo = Date.HighDate;

            return newEntityV;
        }
    }
}
