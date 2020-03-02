using AppInsights.Http.Metrics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AppInsights.Http.Internal.Metrics
{
    internal class Metric : IMetric
    {
        private readonly AppInsightsMetric _appInsightsMetric;

        public Metric(AppInsightsMetric appInsightsMetric, string appInsightsMetricJson, MetricsDefinition metrics)
        {
            _appInsightsMetric = appInsightsMetric;
            var aggregation = JsonConvert.DeserializeObject<JObject>(appInsightsMetricJson)["value"][metrics.ToString()];
            Aggregation = JsonConvert.DeserializeObject<MetricAggregation>(aggregation.ToString());
        }

        public DateTime Start => _appInsightsMetric.Value.Start;

        public DateTime End => _appInsightsMetric.Value.End;

        public IMetricAggregation Aggregation { get; internal set; }
    }
}
