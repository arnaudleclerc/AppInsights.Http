﻿namespace AppInsights.Http.Internal.Http
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AppInsights.Http.Analytics;
    using AppInsights.Http.Configuration;
    using AppInsights.Http.Exceptions;
    using AppInsights.Http.Internal.Analytics;
    using AppInsights.Http.Internal.Authentication;
    using AppInsights.Http.Internal.Metadata;
    using AppInsights.Http.Internal.Metrics;
    using AppInsights.Http.Metadata;
    using AppInsights.Http.Metrics;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;

    internal class AppInsightsHttpClient : IAppInsightsHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEnumerable<AppInsightsConfiguration> _appInsightsConfigurations;

        private readonly AppInsightsRequestAuthenticator _appInsightsRequestAuthenticator;

        public AppInsightsHttpClient(IHttpClientFactory httpClientFactory,
            IOptions<AppInsightsConfiguration> options,
            AppInsightsRequestAuthenticator appInsightsRequestAuthenticator)
        {
            if (string.IsNullOrWhiteSpace(options.Value?.ApplicationId))
            {
                throw new System.ArgumentException("A valid ApplicationId must be provided");
            }
            _httpClientFactory = httpClientFactory;
            _appInsightsConfigurations = new []
            {
                options.Value
            };

            _appInsightsRequestAuthenticator = appInsightsRequestAuthenticator;
        }

        public AppInsightsHttpClient(IHttpClientFactory httpClientFactory,
            AppInsightsConfiguration[] configurations,
            AppInsightsRequestAuthenticator appInsightsRequestAuthenticator)
        {
            var validConfigurations = configurations?.Where(c => !string.IsNullOrWhiteSpace(c?.ApplicationId));
            if (validConfigurations.Count() == 0)
            {
                throw new System.ArgumentException(nameof(configurations), "At least one valid configuration must be provided");
            }

            foreach (var configuration in validConfigurations)
            {
                if (configurations.Count(c => c.ApplicationId == configuration.ApplicationId) > 1)
                {
                    throw new System.ArgumentException($"Multiple configurations where found for the given ApplicationId : {configuration.ApplicationId}");
                }
            }

            _httpClientFactory = httpClientFactory;
            _appInsightsConfigurations = validConfigurations;
            _appInsightsRequestAuthenticator = appInsightsRequestAuthenticator;
        }

        public async Task<IMetric> GetMetricAsync(MetricsDefinition metrics) => await GetMetricAsync(metrics, _appInsightsConfigurations.First().ApplicationId).ConfigureAwait(false);

        public async Task<IMetric> GetMetricAsync(MetricsDefinition metrics, string applicationId)
        {
            var configuration = GetConfigurationByApplicationId(applicationId);
            using(var client = _httpClientFactory.CreateClient(configuration.ApplicationId))
            {
                using(var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.applicationinsights.io/v1/apps/{configuration.ApplicationId}/metrics/{metrics}"))
                {
                    await _appInsightsRequestAuthenticator.AddAuthenticationAsync(request, configuration);
                    var result = await client.SendAsync(request).ConfigureAwait(false);
                    if (!result.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                        var appInsightsException = new AppInsightsException(error["error"]["message"].ToString(), error["error"]["code"].ToString());
                        throw appInsightsException;
                    }
                    var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var metric = JsonConvert.DeserializeObject<AppInsightsMetric>(content);
                    return new Metric(metric, content, metrics);
                }
            }
        }

        public async Task<IMetadata> GetMetadataAsync() => await GetMetadataAsync(_appInsightsConfigurations.First().ApplicationId).ConfigureAwait(false);

        public async Task<IMetadata> GetMetadataAsync(string applicationId)
        {
            var configuration = GetConfigurationByApplicationId(applicationId);
            using(var client = _httpClientFactory.CreateClient(configuration.ApplicationId))
            {
                using(var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.applicationinsights.io/v1/apps/{configuration.ApplicationId}/metrics/metadata"))
                {
                    await _appInsightsRequestAuthenticator.AddAuthenticationAsync(request, configuration);
                    var result = await client.SendAsync(request).ConfigureAwait(false);
                    if (!result.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                        var appInsightsException = new AppInsightsException(error["error"]["message"].ToString(), error["error"]["code"].ToString());
                        throw appInsightsException;
                    }

                    var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                            else if (child.Path.EndsWith(".displayName"))
                            {
                                metadataMetric.DisplayName = child.ToString();
                            }
                        }

                        metricsCollection.Add(metadataMetric);
                    }

                    var dimensions = jsonContent["dimensions"];
                    var dimensionsCollection = new List<IMetadataDimension>();
                    foreach (var dimension in dimensions.Children().Where(dimension => dimension.HasValues))
                    {
                        dimensionsCollection.Add(new MetadataDimension((dimension as JProperty).Name, dimension.First["displayName"].ToString()));
                    }

                    return new Metadata(metricsCollection, dimensionsCollection);
                }

            }
        }

        public async Task<IAnalyticQueryResult> QueryAsync(IAnalyticsQueryBuilder analyticsQueryBuilder) => await QueryAsync(analyticsQueryBuilder, _appInsightsConfigurations.First().ApplicationId).ConfigureAwait(false);

        public async Task<IAnalyticQueryResult> QueryAsync(IAnalyticsQueryBuilder analyticsQueryBuilder, string applicationId)
        {
            var configuration = GetConfigurationByApplicationId(applicationId);
            using(var client = _httpClientFactory.CreateClient(configuration.ApplicationId))
            {
                using(var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.applicationinsights.io/v1/apps/{configuration.ApplicationId}/query"))
                {
                    request.Content = new StringContent("{\"query\": \"" + analyticsQueryBuilder.ToString() + "\"}");
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    await _appInsightsRequestAuthenticator.AddAuthenticationAsync(request, configuration);
                    var result = await client.SendAsync(request).ConfigureAwait(false);
                    if (!result.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                        var appInsightsException = new AppInsightsException(error["error"]["message"].ToString(), error["error"]["code"].ToString());
                        throw appInsightsException;
                    }

                    var analyticsResult = JsonConvert.DeserializeObject<AppInsightsQueryResult>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                    return new AnalyticQueryResult(analyticsResult);
                }
            }
        }

        public async Task<IAnalyticQueryResult> QueryAsync(string query) => await QueryAsync(query, _appInsightsConfigurations.First().ApplicationId).ConfigureAwait(false);

        public async Task<IAnalyticQueryResult> QueryAsync(string query, string applicationId)
        {
            var configuration = GetConfigurationByApplicationId(applicationId);
            using(var client = _httpClientFactory.CreateClient(configuration.ApplicationId))
            {
                using(var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.applicationinsights.io/v1/apps/{configuration.ApplicationId}/query"))
                {
                    request.Content = new StringContent("{\"query\": \"" + query + "\"}");
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    await _appInsightsRequestAuthenticator.AddAuthenticationAsync(request, configuration);
                    var result = await client.SendAsync(request).ConfigureAwait(false);
                    if (!result.IsSuccessStatusCode)
                    {
                        var error = JsonConvert.DeserializeObject<JObject>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                        var appInsightsException = new AppInsightsException(error["error"]["message"].ToString(), error["error"]["code"].ToString());
                        throw appInsightsException;
                    }

                    var analyticsResult = JsonConvert.DeserializeObject<AppInsightsQueryResult>(await result.Content.ReadAsStringAsync().ConfigureAwait(false));
                    return new AnalyticQueryResult(analyticsResult);
                }
            }
        }

        private AppInsightsConfiguration GetConfigurationByApplicationId(string applicationId)
        {
            var configuration = _appInsightsConfigurations.FirstOrDefault(c => c.ApplicationId == applicationId);
            if (configuration == null)
            {
                throw new System.ArgumentException($"Could not find a configuration for the given ApplicationId : {applicationId}");
            }
            return configuration;
        }

    }
}
