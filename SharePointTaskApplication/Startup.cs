using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SharePointTaskApplication.Startup))]
namespace SharePointTaskApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
