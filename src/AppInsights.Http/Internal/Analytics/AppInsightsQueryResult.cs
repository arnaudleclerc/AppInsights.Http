namespace AppInsights.Http.Internal.Analytics
{
    using Newtonsoft.Json;

    internal class AppInsightsQueryResult
    {
        [JsonProperty("tables")]
        public AppInsightsTableQueryResult[] Tables { get; set; }
    }
}
