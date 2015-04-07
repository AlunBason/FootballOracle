using System.Web.Mvc;

namespace FootballOracle.Website.Areas.Acc
{
    public class AccAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Acc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Acc_default",
                "Acc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}