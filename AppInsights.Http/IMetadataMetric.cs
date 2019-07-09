using System.Collections.Generic;

namespace AppInsights.Http
{
    public interface IMetadataMetric
    {
        string Metric { get; }

        IEnumerable<string> SupportedAggregations { get; }

        IEnumerable<string> SupportedGroupBy { get; }

        string DisplayName { get; }

        string DefaultAggregation { get; }

        string Units { get; }
    }
}
