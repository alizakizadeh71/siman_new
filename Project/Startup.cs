using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OPS.Startup))]
namespace OPS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
