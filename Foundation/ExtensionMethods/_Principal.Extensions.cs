using Microsoft.AspNet.Identity;

namespace System.Security.Principal
{
    public static class PrincipalExtensions
    {
        public static Guid GetUserId(this IPrincipal user)
        {
            return new Guid(user.Identity.GetUserId());
        }

        public static bool IsAdmin(this IPrincipal user)
        {
            return user != null && user.IsInRole("Admin");
        }
    }
}
