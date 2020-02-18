using System;
using System.Collections.Generic;
using System.Net.Http;
using AppInsights.Http.Analytics;
using AppInsights.Http.Configuration;
using AppInsights.Http.Internal.Analytics;
using AppInsights.Http.Internal.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                .AddOptions<AppInsightsConfiguration>()
                .Configure(appInsightsConfigurationOptions);

            return services.AddHttpClient()
                .AddSingleton<IAnalyticsQueryBuilderFactory, AnalyticsQueryBuilderFactory>()
                .AddSingleton<IAppInsightsHttpClient>(sp => new AppInsightsHttpClient(
                    sp.GetRequiredService<IHttpClientFactory>(),
                    sp.GetRequiredService<IOptions<AppInsightsConfiguration>>(),
                    sp.GetRequiredService<ILogger>()
                ));
        }

        /// <summary>
        /// Register the services to read telemetry data from the AppInsights API
        /// </summary>
        /// <param name="services">Current services</param>
        /// <param name="appInsightsConfiguration">Configurations to use to conmmunicate with the AppInsights API</param>
        /// <returns>Services collection containing the AppInsights services</returns>
        public static IServiceCollection AddAppInsightsHttpClient(this IServiceCollection services, params AppInsightsConfiguration[] appInsightsConfigurations)
        {
            return services
                .AddHttpClient()
                .AddSingleton<IAnalyticsQueryBuilderFactory, AnalyticsQueryBuilderFactory>()
                .AddSingleton<IAppInsightsHttpClient>(sp => new AppInsightsHttpClient(
                    sp.GetRequiredService<IHttpClientFactory>(),
                    appInsightsConfigurations,
                    sp.GetRequiredService<ILogger>()
                ));
        }
    }
}