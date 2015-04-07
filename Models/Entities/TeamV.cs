using FootballOracle.Foundation;
using FootballOracle.Foundation.Entities;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.RepositoryProviders.Interfaces;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FootballOracle.Models.Entities
{
    public class TeamV : BaseApprovableEntity
    {
        public TeamV()
        {
            TeamNames = new HashSet<TeamName>();
        }

        [StringLength(100)]
        public string TeamName { get; set; }

        public Guid? HomeVenueGuid { get; set; }

        public Guid? CountryGuid { get; set; }

        #region Navigation properties
        public virtual Team Team { get; set; }
        public virtual ICollection<TeamName> TeamNames { get; set; }
        public virtual Country Country { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual Venue HomeVenue { get; set; }
        #endregion
    }

    public static class _TeamVExtensions
    {
        public static IEnumerable<BaseTeamViewModel> ToViewModels(this IEnumerable<TeamV> versions, DateTime viewDate)
        {
            foreach (var version in versions)
                yield return version.ToViewModel(viewDate);
        }

        public static BaseTeamViewModel ToViewModel(this TeamV version, DateTime viewDate)
        {
            return version.ToViewModel<BaseTeamViewModel, Team, TeamV>(viewDate);
        }

        public static TeamV SetData(this TeamV teamV, TeamEditorViewModel viewModel, IRepositoryProvider dbProvider)
        {
            teamV.HeaderKey = viewModel.HeaderKey;
            teamV.HomeVenueGuid = viewModel.HomeVenueGuid;
            teamV.CountryGuid = viewModel.CountryGuid;
            teamV.ResourceGuid = viewModel.ResourceGuid;
            teamV.WebAddress = viewModel.WebAddress;
            teamV.EffectiveFrom = viewModel.EffectiveFrom;
            teamV.EffectiveTo = viewModel.EffectiveTo;
            teamV.DateModified = DateTime.Now;

            teamV.SyncTeamName(dbProvider, viewModel.EditorTeamNameNative, TeamNameType.Primary, LanguageType.Native);
            teamV.SyncTeamName(dbProvider, viewModel.EditorTeamNameEnglish, TeamNameType.Primary, LanguageType.English);
            teamV.SyncTeamName(dbProvider, viewModel.EditorShortname, TeamNameType.ShortName, LanguageType.Native);
            teamV.SyncTeamName(dbProvider, viewModel.EditorNickname, TeamNameType.Nickname, LanguageType.Native);
            teamV.SyncTeamName(dbProvider, viewModel.EditorFullName, TeamNameType.FullName, LanguageType.Native);

            return teamV;
        }

        private static void SyncTeamName(this TeamV teamV, IRepositoryProvider dbProvider, string description, TeamNameType teamNameType, LanguageType languageType)
        {
            var existingEntity = teamV.TeamNames.FirstOrDefault(f => f.TeamNameType == teamNameType && f.LanguageType == languageType);

            if (!string.IsNullOrWhiteSpace(description))
            {
                if (existingEntity != null)
                    existingEntity.Description = description.Trim();
                else
                {
                    var newTeamName = new TeamName()
                    {
                        PrimaryKey = Guid.NewGuid(),
                        TeamVKey = teamV.PrimaryKey,
                        TeamNameType = teamNameType,
                        LanguageType = languageType,
                        Description = description.Trim()
                    };

                    teamV.TeamNames.Add(newTeamName);
                    dbProvider.Attach(teamV);
                }
            }
            else
            {
                if (existingEntity != null)
                    teamV.TeamNames.Remove(existingEntity);
            }
        }

        public static string GetTeamName(this TeamV teamV, TeamNameType teamNameType, LanguageType languageType)
        {
            var teamName = teamV.TeamNames.FirstOrDefault(w => w.TeamNameType == teamNameType && w.LanguageType == languageType);

            return teamName != null ? teamName.Description : string.Empty;
        }
    }
}
