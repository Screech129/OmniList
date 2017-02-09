using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(omnilistService.Startup))]

namespace omnilistService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}