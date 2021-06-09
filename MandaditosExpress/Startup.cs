using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MandaditosExpress.Startup))]
namespace MandaditosExpress
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
