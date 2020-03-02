using Newtonsoft.Json;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AppInsightsTableQueryResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("columns")]
        public AppInsightsTableColumnQueryResult[] Columns { get; set; }
        [JsonProperty("rows")]
        public string[][] Rows { get; set; }
    }
}
