using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HTWeb.Startup))]
namespace HTWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
