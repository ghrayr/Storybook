using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Storybook.Startup))]
namespace Storybook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
