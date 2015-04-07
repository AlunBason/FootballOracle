using System.Web.Mvc;

namespace FootballOracle.Website.Attributes
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public AuthorizeAdminAttribute()
        {
            Roles = "Admin";
        }
    }
}