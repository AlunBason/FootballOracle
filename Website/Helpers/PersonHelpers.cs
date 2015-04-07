using FootballOracle.Foundation;
using FootballOracle.Models.ViewModels.Standard;
using System.Collections.Generic;
using System.Linq;

namespace System.Web.Mvc
{
    public static class PersonHelpers
    {
        public static IHtmlString PersonMatchEvents(this HtmlHelper helper, IEnumerable<MatchEventViewModel> matchEvents)
        {
            var htmlString = string.Empty;
            var booked = false;

            foreach (var item in matchEvents.OrderBy(e => e.Entity.Minute).ThenBy(e => e.Entity.Extra))
            {
                htmlString += helper.MatchEventImage(item, booked);

                if (item.Entity.MatchEventType == MatchEventType.Booked)
                    booked = true;
            }

            return new HtmlString(htmlString);
        }

        public static IHtmlString MatchEventImage(this HtmlHelper helper, MatchEventViewModel matchEventViewModel, bool booked)
        {
            var imagePath = string.Empty;
            var title = string.Empty;

            switch (matchEventViewModel.Entity.MatchEventType)
            {
                case MatchEventType.Booked:
                    imagePath = booked ? "~/images/booked2.png" : "~/images/booked.png";
                    title = "Booked";
                    break;

                case MatchEventType.BroughtOn:
                    imagePath = "~/images/broughton.png";
                    title = "Brought on";
                    break;

                case MatchEventType.OwnGoal:
                    imagePath = "~/images/owngoal.png";
                    title = "Own goal";
                    break;

                case MatchEventType.Scored:
                    imagePath = "~/images/goal.png";
                    title = "Goal";
                    break;

                case MatchEventType.SentOff:
                    imagePath = "~/images/sentoff.png";
                    title = "Sent off";
                    break;

                case MatchEventType.TakenOff:
                    imagePath = "~/images/takenoff.png";
                    title = "Substitued";
                    break;

                default:
                    return new HtmlString(string.Empty);
            }

            var tooltip = matchEventViewModel.Entity.Minute == null
                ? title
                : string.Format("{0}: {1}", title, matchEventViewModel.Entity.Extra == null ? string.Format("{0} minutes", matchEventViewModel.Entity.Minute) : string.Format("{0} (+{1} minutes)", matchEventViewModel.Entity.Minute, matchEventViewModel.Entity.Extra));

            var img = new TagBuilder("img");
            img.MergeAttribute("src", UrlHelper.GenerateContentUrl(imagePath, helper.ViewContext.HttpContext));
            img.MergeAttribute("style", "height: 12px; vertical-align: middle;");
            img.MergeAttribute("hspace", "2");
            img.MergeAttribute("title", tooltip);

            return new HtmlString(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}