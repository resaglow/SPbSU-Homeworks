using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainTickets.Startup))]
namespace TrainTickets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
