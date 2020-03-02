using AppInsights.Http.Metadata;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppInsights.Http.Internal.Metadata
{
    internal class MetadataMetric : IMetadataMetric
    {
        public string Metric { get; internal set; }

        public IEnumerable<string> SupportedAggregations { get; internal set; }

        public IEnumerable<string> SupportedGroupBy { get; internal set; }

        public string DisplayName { get; internal set; }

        public string DefaultAggregation { get; internal set; }

        public string Units { get; internal set; }
    }
}
