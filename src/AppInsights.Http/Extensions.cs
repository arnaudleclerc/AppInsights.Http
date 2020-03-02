namespace AppInsights.Http
{
    using System.Net.Http;
    using System;
    using AppInsights.Http.Analytics;
    using AppInsights.Http.Authentication;
    using AppInsights.Http.Configuration;
    using AppInsights.Http.Internal.Analytics;
    using AppInsights.Http.Internal.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

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
                    new Internal.Authentication.AppInsightsRequestAuthenticator(
                        sp.GetService<IAppInsightsCredentialProvider>()
                    )
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
                    new Internal.Authentication.AppInsightsRequestAuthenticator(
                        sp.GetService<IAppInsightsCredentialProvider>()
                    )
                ));
        }

        /// <summary>
        /// Register the services to read telemetry data from the AppInsights API
        /// </summary>
        /// <param name="services">Current services</param>
        /// <param name="appInsightsConfigurationOptions">Configuration to use to conmmunicate with the AppInsights API</param>
        /// <typeparam name="T">Type to use to get the application insights client secret while using the OAuth flow</typeparam>
        /// <returns>Services collection containing the AppInsights services</returns>
        public static IServiceCollection AddAppInsightsHttpClient<T>(this IServiceCollection services, Action<AppInsightsConfiguration> appInsightsConfigurationOptions)
        where T : class, IAppInsightsCredentialProvider
        {
            services
                .AddOptions<AppInsightsConfiguration>()
                .Configure(appInsightsConfigurationOptions);

            return services.AddHttpClient()
                .AddSingleton<IAnalyticsQueryBuilderFactory, AnalyticsQueryBuilderFactory>()
                .AddSingleton<IAppInsightsCredentialProvider, T>()
                .AddSingleton<IAppInsightsHttpClient>(sp => new AppInsightsHttpClient(
                    sp.GetRequiredService<IHttpClientFactory>(),
                    sp.GetRequiredService<IOptions<AppInsightsConfiguration>>(),
                    new Internal.Authentication.AppInsightsRequestAuthenticator(
                        sp.GetRequiredService<IAppInsightsCredentialProvider>()
                    )
                ));
        }

        /// <summary>
        /// Register the services to read telemetry data from the AppInsights API
        /// </summary>
        /// <param name="services">Current services</param>
        /// <param name="appInsightsConfiguration">Configurations to use to conmmunicate with the AppInsights API</param>
        /// <typeparam name="T">Type to use to get the application insights client secret while using the OAuth flow</typeparam>
        /// <returns>Services collection containing the AppInsights services</returns>
        public static IServiceCollection AddAppInsightsHttpClient<T>(this IServiceCollection services, params AppInsightsConfiguration[] appInsightsConfigurations)
        where T : class, IAppInsightsCredentialProvider
        {
            return services.AddHttpClient()
                .AddSingleton<IAnalyticsQueryBuilderFactory, AnalyticsQueryBuilderFactory>()
                .AddSingleton<IAppInsightsCredentialProvider, T>()
                .AddSingleton<IAppInsightsHttpClient>(sp => new AppInsightsHttpClient(
                    sp.GetRequiredService<IHttpClientFactory>(),
                    appInsightsConfigurations,
                    new Internal.Authentication.AppInsightsRequestAuthenticator(
                        sp.GetRequiredService<IAppInsightsCredentialProvider>()
                    )
                ));
        }
    }
}
