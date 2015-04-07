using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Teams;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Foundation.ViewModels;
using FootballOracle.Foundation;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<TeamV> teamVRepository;
        private DbSet<TeamV> TeamVRepository
        {
            get { return teamVRepository = teamVRepository ?? context.Set<TeamV>(); }
        }

        private IQueryable<TeamV> TeamVs
        {
            get { return TeamVRepository as IQueryable<TeamV>; }
        }

        public void Add(TeamV teamV)
        {
            TeamVRepository.Add(teamV);
        }

        public void Remove(TeamV teamV)
        {
            TeamVRepository.Remove(teamV);
        }

        public void Attach(TeamV teamV)
        {
            context.Entry(teamV).State = EntityState.Modified;
        }

        public async Task<TeamV> GetTeam(Guid teamKey, DateTime viewDate)
        {
            return (await Teams.SingleOrDefaultAsync(f => f.PrimaryKey == teamKey)).GetApprovedVersion(viewDate);
        }

        public async Task<TeamV> GetTeam(Guid primaryKey, Guid teamKey)
        {
            return await TeamVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == teamKey);
        }

        public async Task<IEnumerable<string>> GetTeamAutoCompleteList(Guid userId, bool isAdmin, string searchText)
        {
            var teams = await TeamNames.Where(w => w.Description.Contains(searchText.Trim()) && w.TeamV.IsActive).ToListAsync();
            
            return teams.Select(s => s.Description).Distinct().OrderBy(o => o);
        }

        public async Task<IEnumerable<ICodePickerData>> GetTeamCodePickerData(DateTime viewDate)
        {
            var teamVs = await TeamVs.Where(w => w.IsActive && !w.IsMarkedForDeletion && w.EffectiveFrom <= viewDate && w.EffectiveTo >= viewDate).OrderBy(t => t.TeamName).ToListAsync();
            var teamViewModels = teamVs.ToViewModels(viewDate);

            return teamViewModels.Select(s => new CodePickerViewModel() { Code = s.HeaderKey, Description = s.ToString() });
        }

        public async Task<IEnumerable<BaseTeamViewModel>> GetCountryTeamViewModels(Guid countryHeaderKey, DateTime viewDate)
        {
            var venueKeys = (await GetCountryVenueViewModels(countryHeaderKey, viewDate)).Select(s => s.HeaderKey);

            var teamsWithCountry = await TeamVs.Where(t => t.CountryGuid == countryHeaderKey).ToListAsync();
            var teamsWithVenue = await TeamVs.Where(t => t.CountryGuid == null && venueKeys.Contains((Guid)t.HomeVenueGuid)).ToListAsync();

            return teamsWithCountry.ToViewModels(viewDate).Concat(teamsWithVenue.ToViewModels(viewDate)).OrderBy(t => t.ToString());
        }

        public async Task<IEnumerable<ISearchResult>> SearchTeams(string searchText, DateTime viewDate)
        {
            var teamGroups = await TeamNames
                .Where(w => w.Description.Contains(searchText.Trim()) && w.TeamV.IsActive)
                .Select(s => s.TeamV)
                .GroupBy(g => g.HeaderKey).ToListAsync();

            var teamVs = new List<TeamV>();

            foreach (var teamGroup in teamGroups)
                teamVs.Add(teamGroup.OrderByDescending(t => t.EffectiveFrom).First());

            return teamVs.ToViewModels(viewDate).Cast<ISearchResult>();
        }

        public async Task SyncTeamNames()
        {
            var teamVs = await TeamVs.Where(w => !w.TeamNames.Any()).ToListAsync();

            foreach (var teamV in teamVs)
            {
                Add(new TeamName()
                {
                    PrimaryKey = Guid.NewGuid(),
                    TeamVKey = teamV.PrimaryKey,
                    TeamNameType = TeamNameType.Primary,
                    LanguageType = LanguageType.Native,
                    Description = teamV.TeamName
                });

                SaveChanges();
            }
        }
    }
}
