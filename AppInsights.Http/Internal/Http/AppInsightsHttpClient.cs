using AppInsights.Http.Configuration;
using AppInsights.Http.Exceptions;
using AppInsights.Http.Internal.Metrics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppInsights.Http.Internal.Http
{
    internal class AppInsightsHttpClient : IAppInsightsHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppInsightsConfiguration _appInsightsConfiguration;
        private readonly ILogger _logger;

        public AppInsightsHttpClient(IHttpClientFactory httpClientFactory,
            IOptions<AppInsightsConfiguration> options,
            ILogger logger)
        {
            _httpClientFactory = httpClientFactory;
            _appInsightsConfiguration = options.Value;
            _logger = logger;
        }

        public async Task<IMetric> GetMetricAsync(AppInsights.Http.Metrics metrics)
        {
            using(var client = _httpClientFactory.CreateClient(_appInsightsConfiguration.APIKey))
            {
                using(var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.applicationinsights.io/v1/apps/{_appInsightsConfiguration.ApplicationId}/metrics/{metrics.ToString()}"))
                {
                    request.Headers.TryAddWithoutValidation("x-api-key", _appInsightsConfiguration.APIKey);
                    var result = await client.SendAsync(request);
                    if(!result.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync());
                        var appInsightsException = new AppInsightsException(error["error"]["message"].ToString(), error["error"]["code"].ToString());
                        _logger.LogError($"AppInsightsException - {error["error"]["code"].ToString()} : {error["error"]["message"].ToString()}");
                        throw appInsightsException;
                    }
                    var content = await result.Content.ReadAsStringAsync();
                    var metric = JsonConvert.DeserializeObject<AppInsightsMetric>(content);
                    return new Metric(metric, content, metrics);
                }
            }
        }
    }
}
