using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.Competitions;

namespace FootballOracle.Models.RepositoryProviders
{
    public partial class RepositoryProvider
    {
        private DbSet<CompetitionV> competitionVRepository;
        private DbSet<CompetitionV> CompetitionVRepository
        {
            get { return competitionVRepository = competitionVRepository ?? context.Set<CompetitionV>(); }
        }

        private IQueryable<CompetitionV> CompetitionVs
        {
            get { return CompetitionVRepository as IQueryable<CompetitionV>; }
        }

        public void Add(CompetitionV competitionV)
        {
            CompetitionVRepository.Add(competitionV);
        }

        public async Task<CompetitionV> GetCompetition(Guid competitionKey, DateTime viewDate)
        {
            return (await Competitions.SingleOrDefaultAsync(f => f.PrimaryKey == competitionKey)).GetApprovedVersion(viewDate);
        }

        public async Task<CompetitionV> GetCompetition(Guid primaryKey, Guid competitionKey)
        {
            return await CompetitionVs.SingleOrDefaultAsync(s => s.PrimaryKey == primaryKey && s.HeaderKey == competitionKey);
        }

        public async Task<IEnumerable<string>> GetCompetitionAutoCompleteListAsync(Guid userId, bool isAdmin, string searchText)
        {
            return await CompetitionVs
                .Where(w => w.CompetitionName.Contains(searchText.Trim()) && w.IsActive)
                .Select(s => s.CompetitionName)
                .Distinct()
                .OrderBy(o => o)
                .ToListAsync();
        }

        public async Task<IEnumerable<BaseCompetitionViewModel>> GetCompetitionsByOrganisationAsync(Guid organisationHeaderKey, DateTime viewDate)
        {
            var competitionVs = await CompetitionVs.Where(c => c.OrganisationGuid == organisationHeaderKey).ToListAsync();

            return competitionVs.ToViewModels(viewDate).OrderBy(o => o.EffectiveTo).ThenBy(t => t.ToString());
        }

        public void Remove(CompetitionV competitionV)
        {
            CompetitionVRepository.Remove(competitionV);
        }

        public async Task<IEnumerable<ISearchResult>> SearchCompetitions(string searchText, DateTime viewDate)
        {
            var normalizedText = searchText.RemoveDiacritics();

            var competitionGroups = await CompetitionVs
                .Where(t => (t.CompetitionName.Contains(normalizedText.Trim()) || t.CompetitionName.Contains(searchText.Trim()) && t.IsActive))
                .GroupBy(t => t.HeaderKey).ToListAsync();

            var competitionVs = new List<CompetitionV>();

            foreach (var competitionGroup in competitionGroups)
                competitionVs.Add(competitionGroup.OrderByDescending(t => t.EffectiveFrom).First());

            return competitionVs.ToViewModels(viewDate).Cast<ISearchResult>();
        }
    }
}
