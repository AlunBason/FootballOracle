using FootballOracle.Foundation.Entities;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FootballOracle.Foundation;

namespace FootballOracle.Models.ViewModels.Approvable.Competitions
{
    public class CompetitionEditorViewModel : BaseCompetitionViewModel , IEquatable<CompetitionV>
    {
        #region EditorProperties
        [Display(Name = "Name")]
        public virtual string CompetitionName { get; set; }

        [Display(Name = "Competition type")]
        public CompetitionType? CompetitionType { get; set; }

        [UIHint("DropDowns/Organisation")]
        [Display(Name = "Organisation")]
        public Guid OrganisationGuid { get; set; }

        public virtual int? Rank { get; set; }
        #endregion

        public IEnumerable<BaseCompetitionViewModel> EditableViewModels { get; set; }

        public void GetEntityData()
        {
            CompetitionName = VersionEntity.CompetitionName;
            CompetitionType = VersionEntity.CompetitionType;
            OrganisationGuid = VersionEntity.OrganisationGuid;
            Rank = VersionEntity.Rank;
            WebAddress = VersionEntity.WebAddress;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public bool Equals(CompetitionV other)
        {
            return CompetitionName == other.CompetitionName
                && CompetitionType == other.CompetitionType
                && OrganisationGuid == other.OrganisationGuid
                && Rank == other.Rank
                && WebAddress == other.WebAddress;
        }
    }

    public static class CompetitionEditorViewModelExtensions
    {
        public static CompetitionV ToCompetitionV(this CompetitionEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<CompetitionV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            newEntityV.CompetitionName = editorViewModel.CompetitionName;
            newEntityV.CompetitionType = editorViewModel.CompetitionType;
            newEntityV.OrganisationGuid = editorViewModel.OrganisationGuid;
            newEntityV.Rank = editorViewModel.Rank;
            newEntityV.CampaignPeriodType = PeriodType.Year;
            newEntityV.CampaignPeriodValue = 1;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = editorViewModel.EffectiveFrom;
            newEntityV.EffectiveTo = editorViewModel.EffectiveTo;

            return newEntityV;
        }
    }
}
