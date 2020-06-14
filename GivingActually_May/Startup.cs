using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GivingActually_May.Startup))]
namespace GivingActually_May
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
