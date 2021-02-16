using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BonksList.Startup))]
namespace BonksList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
