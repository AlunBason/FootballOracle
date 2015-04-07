using Microsoft.Owin;
using Owin;
using FootballOracle.Website;

[assembly: OwinStartupAttribute(typeof(Startup))]

namespace FootballOracle.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
