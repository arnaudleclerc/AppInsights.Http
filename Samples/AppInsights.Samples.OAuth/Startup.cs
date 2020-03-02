using System;
using System.Threading.Tasks;
using AppInsights.Http;
using AppInsights.Http.Authentication;
using AppInsights.Http.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly : FunctionsStartup(typeof(AppInsights.Samples.OAuth.Startup))]
namespace AppInsights.Samples.OAuth
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional : true, reloadOnChange : true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services
                .AddSingleton<IAppInsightsCredentialProvider>(new AppInsightsCredentialProvider(config["AppInsightsClientSecret"]))
                .AddAppInsightsHttpClient(options => {
                    options.ApplicationId = config["AppInsightsApplicationId"];
                    options.ClientId = config["AppInsightsClientId"];
                    options.TenantId = config["AppInsightsTenantId"];
                });

            // builder.Services
            //     .AddTransient(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("AppInsights"))
            //     .AddAppInsightsHttpClient(new AppInsightsConfiguration(config["AppInsightsApplicationId"], config["AppInsightsApiKey"]));
        }
    }
}
