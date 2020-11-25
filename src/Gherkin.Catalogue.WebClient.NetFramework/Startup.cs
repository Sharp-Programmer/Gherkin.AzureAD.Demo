using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Gherkin.Catalogue.WebClient.NetFramework.Startup))]

namespace Gherkin.Catalogue.WebClient.NetFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
