using Gherkin.Catalogue.Core;
using Gherkin.Catalogue.FunctionApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Gherkin.Catalogue.FunctionApp
{
    public class Startup : FunctionsStartup
    {


        private IConfiguration Configuration { get; set; }
        IConfidentialClientApplication app;


        public override void Configure(IFunctionsHostBuilder builder)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            ConfigureServices(builder.Services);

        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            var azureAdConfig = Configuration.GetSection(Constants.AzureAdConfigSectionName).Get<AzureAdConfig>();

            services.Configure<ProductApiSettings>(Configuration.GetSection(Constants.ProductApiConfigSectionName));

            app = ConfidentialClientApplicationBuilder.Create(azureAdConfig.ClientId)
                .WithClientSecret(azureAdConfig.ClientSecret)
                .WithAuthority($"{azureAdConfig.Instance}{azureAdConfig.TenantId}/")
                .Build();

            services.AddTransient(a => app);

            var productApi = Configuration.GetSection(Constants.ProductApiConfigSectionName).Get<ProductApiSettings>();

            services.AddHttpClient(Constants.ProductClientName, client =>
            {
                client.BaseAddress = productApi.BaseAddress;
            });



        }
    }
}
