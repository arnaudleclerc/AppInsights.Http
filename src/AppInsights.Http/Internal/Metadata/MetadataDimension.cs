namespace AppInsights.Http.Internal.Metadata
{
    using AppInsights.Http.Metadata;

    internal class MetadataDimension : IMetadataDimension
    {
        public string Metric { get; }
        public string DisplayName { get; }
        public MetadataDimension(string metric, string displayName)
        {
            Metric = metric;
            DisplayName = displayName;
        }

    }
}
