using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DVD_Rent.Startup))]
namespace DVD_Rent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
