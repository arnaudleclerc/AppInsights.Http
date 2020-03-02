namespace AppInsights.Http.Internal.Metrics
{
    using System;
    using Newtonsoft.Json;

    internal class AppInsightsMetric
    {
        [JsonProperty("value")]
        public AppInsightsMetricValue Value { get; set; }
    }

    internal class AppInsightsMetricValue
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }
    }
}
