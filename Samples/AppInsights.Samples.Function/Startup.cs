using AppInsights.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

[assembly: FunctionsStartup(typeof(AppInsights.Samples.Function.Startup))]
namespace AppInsights.Samples.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services
                .AddTransient(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger("AppInsights"))
                .AddAppInsightsHttpClient(appInsightsConfig =>
                {
                    appInsightsConfig.APIKey = config["AppInsightsApiKey"];
                    appInsightsConfig.ApplicationId = config["AppInsightsApplicationId"];
                });
        }
    }
}
