using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCSampleNinject.Startup))]
namespace MVCSampleNinject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
