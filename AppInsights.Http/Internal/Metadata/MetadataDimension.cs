using AppInsights.Http.Metadata;

namespace AppInsights.Http.Internal.Metadata
{
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
