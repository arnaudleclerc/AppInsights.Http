using AppInsights.Http.Analytics;
using AppInsights.Http.Configuration;
using AppInsights.Http.Internal.Analytics;
using AppInsights.Http.Internal.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AppInsights.Http
{
    public static class Extensions
    {
        /// <summary>
        /// Register the services to read telemetry data from the AppInsights API
        /// </summary>
        /// <param name="services">Current services</param>
        /// <param name="appInsightsConfigurationOptions">Configuration to use to conmmunicate with the AppInsights API</param>
        /// <returns>Services collection containing the AppInsights services</returns>
        public static IServiceCollection AddAppInsightsHttpClient(this IServiceCollection services, Action<AppInsightsConfiguration> appInsightsConfigurationOptions)
        {
            services
                .AddHttpClient()
                .AddSingleton<IAppInsightsHttpClient, AppInsightsHttpClient>()
                .AddSingleton<IAnalyticsQueryBuilderFactory, AnalyticsQueryBuilderFactory>()
                .AddOptions<AppInsightsConfiguration>()
                .Configure(appInsightsConfigurationOptions);

            return services;
        }
    }
}
