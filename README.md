[![Build status](https://aleclerc.visualstudio.com/azure-appinsights-api-wrapper/_apis/build/status/AppInsights.Http%20-%20Nuget)](https://aleclerc.visualstudio.com/azure-appinsights-api-wrapper/_build/latest?definitionId=6) [![NuGet](https://img.shields.io/nuget/v/AppInsights.Http.svg)](https://www.nuget.org/packages/AppInsights.Http/)

# AppInsights.Http

Library allowing to retrieve data from the Application Insights API.

## Authentication to the API

So far, only the API key authentication is supported. 

## Registering the services

An extension method `AddAppInsightsHttpClient` allows you to register the services of this library on the service collection container. You can pass the configuration to use to authenticate to the Application Insights API while calling this method.

You can register one applications insights to be consumed.

```
namespace Demo
{
    using AppInsights.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppInsightsHttpClient(options => { 
                options.APIKey = Configuration["AppInsights:ApiKey"];
                options.ApplicationId = Configuration["AppInsights:ApplicationId"];
            });
        }
    }
}
```

Or you can register multiple ones.

```
namespace Demo
{
    using AppInsights.Http;
    using AppInsights.Http.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppInsightsHttpClient(
                new AppInsightsConfiguration("firstApplicationId", "firstApiKey")
                , new AppInsightsConfiguration("secondApplicationId", "secondApiKey")
            );
        }
    }
}
```

## Querying the data

You can build your query using the predefined methods of an `IAnalyticsQueryBuilder` provided by `IAnalyticsQueryBuilderFactory`.

```
namespace Demo
{
    using System.Threading.Tasks;
    using AppInsights.Http.Analytics;
    using AppInsights.Http;

    public class HealthService
    {
        private readonly IAppInsightsHttpClient _appInsightsHttpClient;
        private readonly IAnalyticsQueryBuilderFactory _analyticsQueryBuilderFactory;

        public HealthService(IAppInsightsHttpClient appInsightsHttpClient,
            IAnalyticsQueryBuilderFactory analyticsQueryBuilderFactory)
        {
            _appInsightsHttpClient = appInsightsHttpClient;
            _analyticsQueryBuilderFactory = analyticsQueryBuilderFactory;
        }

        public async Task Get_WithQueryBuilder()
        {
            var query = _analyticsQueryBuilderFactory.CreateAnalyticsQueryBuilder(AnalyticsSchema.Requests)
                .WithTimestampFilter(AnalyticFilterOperator.SuperiorOrEqual, AnalyticTimestampFilterOperator.Ago, AnalyticTimestampDuration.Day)
                .WithFilter("cloud_RoleName", AnalyticFilterOperator.Equal, "application-name")
                .WithSummarizeCount("name", "resultCode", "cloud_RoleName", "success", "timestamp", "duration");

            var result = await _appInsightsHttpClient.QueryAsync(query);
        }
    }
}
```

Or by sending directly the request to execute.

```
namespace Demo
{
    using System.Threading.Tasks;
    using AppInsights.Http;

    public class HealthService
    {
        private readonly IAppInsightsHttpClient _appInsightsHttpClient;

        public HealthService(IAppInsightsHttpClient appInsightsHttpClient)
        {
            _appInsightsHttpClient = appInsightsHttpClient;
        }

        public async Task Get_WithQuery()
        {
            var query = $"requests | where timestamp >= ago(1d) | where cloud_RoleName == 'application-name' | summarize count() by name, resultCode, cloud_RoleName, success, timestamp, duration";
            var result = await _appInsightsHttpClient.QueryAsync(query);
        }
    }
}
```

If you registered multiple instances of application insights during the registration, you can give the application ID you want to use while calling the QueryAsync method.

```
namespace Demo
{
    using System.Threading.Tasks;
    using AppInsights.Http;

    public class HealthService
    {
        private readonly IAppInsightsHttpClient _appInsightsHttpClient;

        public HealthService(IAppInsightsHttpClient appInsightsHttpClient)
        {
            _appInsightsHttpClient = appInsightsHttpClient;
        }

        public async Task Get_WithQuery()
        {
            var query = $"requests | where timestamp >= ago(1d) | where cloud_RoleName == 'application-name' | summarize count() by name, resultCode, cloud_RoleName, success, timestamp, duration";
            var result = await _appInsightsHttpClient.QueryAsync(query, "appInsightsApplicationId");
        }
    }
}
```