using System.Web.Mvc.Html;
using FootballOracle.Foundation;
using FootballOracle.Foundation.Interfaces;
using FootballOracle.Models.Entities;
using FootballOracle.Models.ViewModels.Approvable.People;
using FootballOracle.Models.ViewModels.Approvable.Competitions;

namespace System.Web.Mvc
{
    public static class ActionLinkHelpers
    {
        public static IHtmlString ApprovalTypeLink(this HtmlHelper helper, IApprovableLinkData approvableLinkData)
        {
            return helper.ApprovalTypeLink(approvableLinkData.AreaType, approvableLinkData.ToString(), approvableLinkData.HeaderKey, approvableLinkData.ViewDate, null);
        }

        public static IHtmlString ApprovalTypeLink(this HtmlHelper helper, AreaType areaType, string displayName, Guid headerKey)
        {
            return helper.ApprovalTypeLink(areaType, displayName, headerKey, DateTime.Now, null);
        }

        public static IHtmlString ApprovalTypeLink(this HtmlHelper helper, AreaType areaType, string displayName, Guid headerKey, DateTime viewDate)
        {
            return helper.ApprovalTypeLink(areaType, displayName, headerKey, viewDate, null);
        }

        public static IHtmlString ApprovalTypeLink(this HtmlHelper helper, AreaType areaType, string displayName, Guid headerKey, DateTime viewDate, object htmlAttributes)
        {
            if (viewDate.Date != DateTime.Today)
                return helper.ActionLink(displayName ?? headerKey.ToString(), "Index", "Details", new { @area = areaType.ToString(), hk = headerKey.ToShortGuid(), dt = viewDate.ToUrlString() }, htmlAttributes);
            else
                return helper.ActionLink(displayName ?? headerKey.ToString(), "Index", "Details", new { @area = areaType.ToString(), hk = headerKey.ToShortGuid() }, htmlAttributes);
        }

        public static IHtmlString ByDateLink(this HtmlHelper html, DateTime viewDate, object htmlAttributes, string dateFormat = "")
        {
            var dateDisplay = string.IsNullOrWhiteSpace(dateFormat) ? viewDate.ToDisplayString() : viewDate.ToString(dateFormat);

            return html.ActionLink(dateDisplay, "ByDate", "Home", new { @area = string.Empty, dt = viewDate.ToUrlString() }, htmlAttributes);
        }

        public static IHtmlString DisplayBadgeWithTeamLink(this HtmlHelper helper, Team team, DateTime viewDate, int imageWidth, int imageHeight, object htmlAttributes, bool isRtl = false, bool teamShortNames = false)
        {
            var teamViewModel = team.ToViewModel(viewDate);

            var teamDisplay = string.Format("<span style=\"vertical-align:middle;\">{0}</span>", helper.ApprovalTypeLink(AreaType.Tms, teamShortNames ? teamViewModel.ShortName : teamViewModel.ToString(), teamViewModel.HeaderKey, viewDate, htmlAttributes).ToString());

            if (teamViewModel.VersionEntity.Resource != null)
            {
                if (!isRtl)
                    teamDisplay = string.Format("{0}&nbsp;{1}", helper.BytesToImage(teamViewModel.VersionEntity.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", teamViewModel.ToString())), teamDisplay);
                else
                    teamDisplay = string.Format("{1}&nbsp;{0}", helper.BytesToImage(teamViewModel.VersionEntity.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", teamViewModel.ToString())), teamDisplay);
            }

            return new HtmlString(teamDisplay);
        }

        public static IHtmlString DisplayTeamBadgeWithPersonLink(this HtmlHelper helper, Person person, DateTime viewDate, int imageWidth, int imageHeight, object htmlAttributes, bool isRtl = false)
        {
            var personViewModel = person.ToViewModel(viewDate);
            var teamViewModel = personViewModel.TeamViewModel(viewDate);

            var personDisplay = string.Format("<span style=\"vertical-align:middle;\">{0}</span>", helper.ApprovalTypeLink(AreaType.Ppl, personViewModel.ToString(), personViewModel.HeaderKey, viewDate, htmlAttributes).ToString());

            if (teamViewModel.VersionEntity.Resource != null)
            {
                if (!isRtl)
                    personDisplay = string.Format("{0}&nbsp;{1}", helper.BytesToImage(teamViewModel.VersionEntity.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", teamViewModel.ToString())), personDisplay);
                else
                    personDisplay = string.Format("{1}&nbsp;{0}", helper.BytesToImage(teamViewModel.VersionEntity.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", teamViewModel.ToString())), personDisplay);
            }

            return new HtmlString(personDisplay);
        }

        public static IHtmlString DisplayFlagWithCountryLink(this HtmlHelper helper, Country country, DateTime viewDate, int imageWidth, int imageHeight, object htmlAttributes, bool isRtl = false)
        {
            var countryV = country.GetApprovedVersion<CountryV>(viewDate);

            var countryDisplay = string.Format("<span style=\"vertical-align:middle;\">{0}</span>", helper.ApprovalTypeLink(AreaType.Cnt, countryV.CountryName, countryV.HeaderKey, viewDate, htmlAttributes).ToString());

            if (countryV.Resource != null)
            {
                if (!isRtl)
                    countryDisplay = string.Format("{0}&nbsp;{1}", helper.BytesToImage(countryV.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", countryV.CountryName)), countryDisplay);
                else
                    countryDisplay = string.Format("{1}&nbsp;{0}", helper.BytesToImage(countryV.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", countryV.CountryName)), countryDisplay);
            }

            return new HtmlString(countryDisplay);
        }

        public static IHtmlString DisplayCountryFlagWithCompetitionLink(this HtmlHelper helper, Campaign campaign, DateTime viewDate, int imageWidth, int imageHeight, object htmlAttributes, bool isRtl = false)
        {
            var competitionViewModel = campaign.Competition.ToViewModel(viewDate);
            var countryV = competitionViewModel.VersionEntity.Competition.GetParentCountry(viewDate);

            var countryDisplay = string.Format("<span style=\"vertical-align:middle;\">{0}</span>", helper.ApprovalTypeLink(AreaType.Cmp, competitionViewModel.ToString(), campaign.CompetitionKey, viewDate, htmlAttributes).ToString());

            if (countryV != null && countryV.Resource != null)
            {
                if (!isRtl)
                    countryDisplay = string.Format("{0}&nbsp;{1}", helper.BytesToImage(countryV.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", countryV.CountryName)), countryDisplay);
                else
                    countryDisplay = string.Format("{1}&nbsp;{0}", helper.BytesToImage(countryV.Resource.ResourceBytes, imageWidth, imageHeight, string.Format("{0} badge", countryV.CountryName)), countryDisplay);
            }

            return new HtmlString(countryDisplay);
        }
    }
}