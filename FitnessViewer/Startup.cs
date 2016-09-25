using FitnessViewer.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FitnessViewer.Startup))]
namespace FitnessViewer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AutoMapperConfig.Configure();
        }
    }
}
