using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RegistrationAdvisory.Startup))]
namespace RegistrationAdvisory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
