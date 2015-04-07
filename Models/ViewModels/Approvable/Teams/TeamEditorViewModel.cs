using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FootballOracle.Models.ViewModels.Approvable.Teams
{
    public class TeamEditorViewModel : BaseTeamViewModel, IEquatable<TeamV>
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(25)]
        [Display(Name = "Team name (Native language)")]
        public virtual string EditorTeamNameNative { get; set; }

        [StringLength(25)]
        [Display(Name = "Team name (English language)")]
        public virtual string EditorTeamNameEnglish { get; set; }

        [Display(Name = "Full name")]
        public virtual string EditorFullName { get; set; }

        [StringLength(10)]
        [Display(Name = "Short name")]
        public virtual string EditorShortname { get; set; }

        [Display(Name = "Nickname")]
        public virtual string EditorNickname { get; set; }

        [Display(Name = "Home venue")]
        [UIHint("DropDowns/Venue")]
        public Guid? HomeVenueGuid { get; set; }

        [Display(Name = "Country")]
        [UIHint("DropDowns/Country")]
        public Guid? CountryGuid { get; set; }

        [Display(Name = "Image")]
        [UIHint("Image")]
        public Guid? ResourceGuid { get; set; }

        public IEnumerable<BaseTeamViewModel> EditableViewModels { get; set; }

        public void GetEntityData()
        {
            EditorTeamNameNative = VersionEntity.GetTeamName(TeamNameType.Primary, LanguageType.Native);
            EditorTeamNameEnglish = VersionEntity.GetTeamName(TeamNameType.Primary, LanguageType.English);
            EditorFullName = VersionEntity.GetTeamName(TeamNameType.FullName, LanguageType.Native);
            EditorShortname = VersionEntity.GetTeamName(TeamNameType.ShortName, LanguageType.Native);
            EditorNickname = VersionEntity.GetTeamName(TeamNameType.Nickname, LanguageType.Native);
            ResourceGuid = VersionEntity.ResourceGuid;
            HomeVenueGuid = VersionEntity.HomeVenueGuid;
            CountryGuid = VersionEntity.CountryGuid;
            WebAddress = VersionEntity.WebAddress;
            EffectiveFrom = VersionEntity.EffectiveFrom;
            EffectiveTo = VersionEntity.EffectiveTo;
        }

        public bool Equals(TeamV other)
        {
            if (CountryGuid != other.CountryGuid || HomeVenueGuid != other.HomeVenueGuid || ResourceGuid != other.ResourceGuid || WebAddress != other.WebAddress)
                return false;

            if (!TeamNameEquals(other, EditorTeamNameNative, TeamNameType.Primary, LanguageType.Native))
                return false;

            if (!TeamNameEquals(other, EditorTeamNameEnglish, TeamNameType.Primary, LanguageType.English))
                return false;

            if (!TeamNameEquals(other, EditorShortname, TeamNameType.ShortName, LanguageType.Native))
                return false;

            if (!TeamNameEquals(other, EditorNickname, TeamNameType.Nickname, LanguageType.Native))
                return false;

            if (!TeamNameEquals(other, EditorFullName, TeamNameType.FullName, LanguageType.Native))
                return false;

            return true;
        }

        private bool TeamNameEquals(TeamV other, string description, TeamNameType teamNameType, LanguageType languageType)
        {
             var existingEntity = other.TeamNames.FirstOrDefault(f => f.TeamNameType == teamNameType && f.LanguageType == languageType);

             if (!string.IsNullOrWhiteSpace(description))
            {
                if (existingEntity == null)
                    return false;

                if (existingEntity.Description != description)
                    return false;
            }
            else
            {
                if (existingEntity != null)
                    return false;
            }

            return true;
        }
    }

    public static class _TeamEditorViewModelExtensions
    {
        public static TeamV ToTeamV(this TeamEditorViewModel editorViewModel, Guid ownerUserId, Guid modifiedUserId)
        {
            var newEntityV = BaseApprovableEntity.CreateNewVersion<TeamV>(ownerUserId, modifiedUserId);
            newEntityV.HeaderKey = editorViewModel.HeaderKey;
            //newEntityV.TeamName = editorViewModel.TeamName;
            newEntityV.HomeVenueGuid = editorViewModel.HomeVenueGuid;
            newEntityV.CountryGuid = editorViewModel.CountryGuid;
            newEntityV.ResourceGuid = editorViewModel.ResourceGuid;
            newEntityV.WebAddress = editorViewModel.WebAddress;
            newEntityV.EffectiveFrom = editorViewModel.EffectiveFrom;
            newEntityV.EffectiveTo = editorViewModel.EffectiveTo;

            var teamNames = new List<TeamName>();

            if (!string.IsNullOrWhiteSpace(editorViewModel.EditorTeamNameNative))
                teamNames.Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = newEntityV.PrimaryKey,
                    TeamNameType = TeamNameType.Primary,
                    LanguageType = LanguageType.Native,
                    Description = editorViewModel.EditorTeamNameNative.Trim()
                });

            if (!string.IsNullOrWhiteSpace(editorViewModel.EditorTeamNameEnglish))
                teamNames.Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = newEntityV.PrimaryKey,
                    TeamNameType = TeamNameType.Primary,
                    LanguageType = LanguageType.English,
                    Description = editorViewModel.EditorTeamNameEnglish.Trim()
                });

            if (!string.IsNullOrWhiteSpace(editorViewModel.EditorShortname))
                teamNames.Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = newEntityV.PrimaryKey,
                    TeamNameType = TeamNameType.ShortName,
                    LanguageType = LanguageType.Native,
                    Description = editorViewModel.EditorShortname.Trim()
                });

            if (!string.IsNullOrWhiteSpace(editorViewModel.EditorNickname))
                teamNames.Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = newEntityV.PrimaryKey,
                    TeamNameType = TeamNameType.Nickname,
                    LanguageType = LanguageType.Native,
                    Description = editorViewModel.EditorNickname.Trim()
                });

            if (!string.IsNullOrWhiteSpace(editorViewModel.EditorFullName))
                teamNames.Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = newEntityV.PrimaryKey,
                    TeamNameType = TeamNameType.FullName,
                    LanguageType = LanguageType.Native,
                    Description = editorViewModel.EditorFullName.Trim()
                });

            newEntityV.TeamNames = teamNames;

            return newEntityV;
        }
    }
}
