[![Build status](https://aleclerc.visualstudio.com/azure-appinsights-api-wrapper/_apis/build/status/AppInsights.Http%20-%20Nuget)](https://aleclerc.visualstudio.com/azure-appinsights-api-wrapper/_build/latest?definitionId=6) [![NuGet](https://img.shields.io/nuget/v/AppInsights.Http.svg)](https://www.nuget.org/packages/AppInsights.Http/)

# AppInsights.Http

Library allowing to retrieve data from the Application Insights API.

## Authentication to the API

### Using OAuth with client_credentials

You can authenticate against the Application Insights API using OAuth with client_credentials. To do so, you need to provide :

- The Application Id of Application Insights
- ClientId of the application registered on AAD
- TenantId of your AAD Instance
- An implementation of `IAppInsightsCredentialProvider` to retrieve your client secret. You can register this service directly on IServiceCollection or by calling the generic `AddAppInsightsHttpClient` with the type you want to register as argument.

A sample is available at /samples/AppInsights.Samples.Oauth

```
Services.AddSingleton<IAppInsightsCredentialProvider>(new AppInsightsCredentialProvider(config["AppInsightsClientSecret"]))
                .AddAppInsightsHttpClient(options => {
                    options.ApplicationId = config["AppInsightsApplicationId"];
                    options.ClientId = config["AppInsightsClientId"];
                    options.TenantId = config["AppInsightsTenantId"];
                });
```

```
namespace AppInsights.Samples.OAuth
{
    using System.Threading.Tasks;
    using AppInsights.Http.Authentication;

    public class AppInsightsCredentialProvider : IAppInsightsCredentialProvider
    {
        private readonly string _clientSecret;

        public AppInsightsCredentialProvider(string clientSecret)
        {
            _clientSecret = clientSecret;
        }

        public Task<string> GetClientSecretAsync()
        {
            return Task.FromResult(_clientSecret);
        }
    }
}

```

### With ApplicationId and API Key

This authentication simply puts a `x-api-key` header with your API Key on the request to Application Insights. To do so, you need to provide :

- The Application Id of Application Insights
- The API Key generated on Application Insights

A sample is available at /samples/AppInsights.Samples.Function

```
Services.AddAppInsightsHttpClient(appInsightsConfig =>
                {
                    appInsightsConfig.APIKey = config["AppInsightsApiKey"];
                    appInsightsConfig.ApplicationId = config["AppInsightsApplicationId"];
                });
```

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