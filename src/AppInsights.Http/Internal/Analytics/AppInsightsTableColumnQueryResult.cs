using Newtonsoft.Json;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AppInsightsTableColumnQueryResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
