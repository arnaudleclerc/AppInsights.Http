namespace AppInsights.Http.Internal.Analytics
{
    using Newtonsoft.Json;

    internal class AppInsightsTableColumnQueryResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
