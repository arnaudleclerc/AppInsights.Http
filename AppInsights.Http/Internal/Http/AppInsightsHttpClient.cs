using AppInsights.Http.Configuration;
using AppInsights.Http.Exceptions;
using AppInsights.Http.Internal.Metadata;
using AppInsights.Http.Internal.Metrics;
using AppInsights.Http.Metadata;
using AppInsights.Http.Metrics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IMetric> GetMetricAsync(MetricsDefinition metrics)
        {
            using (var client = _httpClientFactory.CreateClient(_appInsightsConfiguration.APIKey))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.applicationinsights.io/v1/apps/{_appInsightsConfiguration.ApplicationId}/metrics/{metrics.ToString()}"))
                {
                    request.Headers.TryAddWithoutValidation("x-api-key", _appInsightsConfiguration.APIKey);
                    var result = await client.SendAsync(request);
                    if (!result.IsSuccessStatusCode)
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

        public async Task<IMetadata> GetMetadataAsync()
        {
            using (var client = _httpClientFactory.CreateClient(_appInsightsConfiguration.APIKey))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.applicationinsights.io/v1/apps/{_appInsightsConfiguration.ApplicationId}/metrics/metadata"))
                {
                    request.Headers.TryAddWithoutValidation("x-api-key", _appInsightsConfiguration.APIKey);
                    var result = await client.SendAsync(request);
                    if (!result.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync());
                        var appInsightsException = new AppInsightsException(error["error"]["message"].ToString(), error["error"]["code"].ToString());
                        _logger.LogError($"AppInsightsException - {error["error"]["code"].ToString()} : {error["error"]["message"].ToString()}");
                        throw appInsightsException;
                    }

                    var content = await result.Content.ReadAsStringAsync();
                    var jsonContent = JsonConvert.DeserializeObject<JObject>(content);
                    var metrics = jsonContent["metrics"];

                    var metricsCollection = new List<IMetadataMetric>();

                    foreach (var metric in metrics.Children().Where(metric => metric.HasValues))
                    {

                        var metadataMetric = new MetadataMetric
                        {
                            Metric = (metric as JProperty).Name
                        };

                        foreach (var child in metric.First.Values())
                        {
                            if (child.Path.EndsWith(".supportedAggregations"))
                            {
                                metadataMetric.SupportedAggregations = JsonConvert.DeserializeObject<string[]>(child.ToString());
                            }
                            else if (child.Path.EndsWith(".supportedGroupBy"))
                            {
                                metadataMetric.SupportedGroupBy = JsonConvert.DeserializeObject<string[]>(child["all"].ToString());
                            }
                            else if (child.Path.EndsWith(".units"))
                            {
                                metadataMetric.Units = child.ToString();
                            }
                            else if (child.Path.EndsWith(".defaultAggregation"))
                            {
                                metadataMetric.DefaultAggregation = child.ToString();
                            }
                            else if(child.Path.EndsWith(".displayName"))
                            {
                                metadataMetric.DisplayName = child.ToString();
                            }
                        }

                        metricsCollection.Add(metadataMetric);
                    }

                    var dimensions = jsonContent["dimensions"];
                    var dimensionsCollection = new List<IMetadataDimension>();
                    foreach(var dimension in dimensions.Children().Where(dimension => dimension.HasValues))
                    {
                        dimensionsCollection.Add(new MetadataDimension((dimension as JProperty).Name, dimension.First["displayName"].ToString()));
                    }

                    return new Metadata.Metadata(metricsCollection, dimensionsCollection);
                }

            }
        }

    }
}
