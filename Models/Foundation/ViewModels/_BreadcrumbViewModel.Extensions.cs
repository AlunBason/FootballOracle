using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.People;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Foundation.ViewModels
{
    public static class BreadcrumbViewModelExtensions
    {
        public static void CompetitionBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, CompetitionV competitionV, DateTime viewDate)
        {
            breadcrumbViewModels.OrganisationBreadcrumb(competitionV.Organisation.GetApprovedVersion<OrganisationV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Cmp, competitionV.HeaderKey, competitionV.CompetitionName, string.Empty);
        }

        public static void CountryBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, CountryV countryV, DateTime viewDate)
        {
            breadcrumbViewModels.OrganisationBreadcrumb(countryV.Organisation.GetApprovedVersion<OrganisationV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Cnt, countryV.HeaderKey, countryV.CountryName, string.Empty);
        }

        public static void MatchBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, MatchV matchV, DateTime viewDate)
        {
            breadcrumbViewModels.CompetitionBreadcrumb(matchV.CampaignStage.Campaign.Competition.GetApprovedVersion<CompetitionV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Mtc, matchV.HeaderKey, matchV.ToViewModel(viewDate).ToString(), string.Empty);
        }

        public static void OrganisationBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, OrganisationV organisationV, DateTime viewDate)
        {
            if (organisationV.CountryGuid != null)
                breadcrumbViewModels.CountryBreadcrumb(organisationV.Country.GetApprovedVersion<CountryV>(viewDate), viewDate);

            if (organisationV.ParentOrganisationGuid != null)
                breadcrumbViewModels.OrganisationBreadcrumb(organisationV.ParentOrganisation.GetApprovedVersion<OrganisationV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Org, organisationV.HeaderKey, organisationV.OrganisationName, organisationV.OrganisationDescription);
        }

        public static void PersonBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, PersonV personV, DateTime viewDate)
        {
            var personViewModel = personV.ToViewModel(viewDate);

            var matchEventVs = personViewModel.VersionEntity.Person.MatchEvents.OrderByDescending(m => m.MatchV.MatchDate).Take(1);

            if (matchEventVs.Any())
                breadcrumbViewModels.TeamBreadcrumb(matchEventVs.FirstOrDefault().Team.GetApprovedVersion<TeamV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Ppl, personViewModel.HeaderKey, personViewModel.ToString(), string.Empty);
        }

        public static void TeamBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, TeamV teamV, DateTime viewDate)
        {
            var lastMatchV = teamV.Team.Team1MatchVs.Where(m => m.CampaignStage.IsDefault && m.CampaignStage.IsLeague).Union(teamV.Team.Team2MatchVs.Where(m => m.CampaignStage.IsDefault && m.CampaignStage.IsLeague))
                .OrderByDescending(o => o.MatchDate)
                .FirstOrDefault(w => w.MatchDate <= viewDate);

            if (lastMatchV != null)
                breadcrumbViewModels.CompetitionBreadcrumb(lastMatchV.CampaignStage.Campaign.Competition.GetApprovedVersion<CompetitionV>(viewDate), viewDate);
            else if (teamV.CountryGuid != null)
                breadcrumbViewModels.CountryBreadcrumb(teamV.Country.GetApprovedVersion<CountryV>(viewDate), viewDate);
            else if (teamV.HomeVenueGuid != null)
                breadcrumbViewModels.CountryBreadcrumb(teamV.HomeVenue.GetApprovedVersion<VenueV>(viewDate).Country.GetApprovedVersion<CountryV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Tms, teamV.HeaderKey, teamV.ToViewModel(viewDate).ToString(), string.Empty);
        }

        public static void VenueBreadcrumb(this IList<BreadcrumbViewModel> breadcrumbViewModels, VenueV venueV, DateTime viewDate)
        {
            breadcrumbViewModels.CountryBreadcrumb(venueV.Country.GetApprovedVersion<CountryV>(viewDate), viewDate);

            breadcrumbViewModels.Add(AreaType.Ven, venueV.PrimaryKey, venueV.VenueName, string.Empty);
        }

        private static void Add(this IList<BreadcrumbViewModel> breadcrumbViewModels, AreaType area, Guid headerKey, string description, string tooltip)
        {
            breadcrumbViewModels.Add(new BreadcrumbViewModel()
            {
                Hk = headerKey.ToShortGuid(),
                Area = area,
                Description = description,
                Tooltip = tooltip
            });
        }
    }
}
