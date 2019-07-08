using Newtonsoft.Json;

namespace Azure.AppInsights.API.Internal.Metrics
{
    internal class MetricAggregation : IMetricAggregation
    {
        [JsonProperty("min")]
        public long? Min { get; set; }

        [JsonProperty("max")]
        public long? Max { get; set; }

        [JsonProperty("avg")]
        public long? Avg { get; set; }

        [JsonProperty("stddev")]
        public long? StdDev { get; set; }
        [JsonProperty("sum")]
        public long? Sum { get; set; }
    }
}
