using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Refma5neo.Startup))]
namespace Refma5neo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
