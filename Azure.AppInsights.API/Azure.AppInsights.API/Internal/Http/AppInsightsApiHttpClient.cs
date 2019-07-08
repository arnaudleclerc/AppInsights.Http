using Azure.AppInsights.API.Configuration;
using Azure.AppInsights.API.Internal.Metrics;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Azure.AppInsights.API.Internal.Http
{
    internal class AppInsightsApiHttpClient : IAppInsightsHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppInsightsConfiguration _appInsightsConfiguration;

        public AppInsightsApiHttpClient(IHttpClientFactory httpClientFactory,
            IOptions<AppInsightsConfiguration> options)
        {
            _httpClientFactory = httpClientFactory;
            _appInsightsConfiguration = options.Value;
        }

        public async Task<IMetric> GetMetricAsync(Azure.AppInsights.API.Metrics metrics)
        {
            using(var client = _httpClientFactory.CreateClient(_appInsightsConfiguration.APIKey))
            {
                using(var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.applicationinsights.io/v1/apps/{_appInsightsConfiguration.ApplicationId}/metrics/{metrics.ToString()}"))
                {
                    request.Headers.TryAddWithoutValidation("x-api-key", _appInsightsConfiguration.APIKey);
                    var result = await client.SendAsync(request);
                    if(!result.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var content = await result.Content.ReadAsStringAsync();
                    var metric = JsonConvert.DeserializeObject<AppInsightsMetric>(content);
                    return new Metric(metric, content, metrics);
                }
            }
        }
    }
}
