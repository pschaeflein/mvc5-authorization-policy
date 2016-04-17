using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCSampleIndividualAccount.Startup))]
namespace MVCSampleIndividualAccount
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
