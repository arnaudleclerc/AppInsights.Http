using Newtonsoft.Json;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AppInsightsQueryResult
    {
        [JsonProperty("tables")]
        public AppInsightsTableQueryResult[] Tables { get; set; }
    }
}
