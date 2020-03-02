namespace AppInsights.Http.Metadata
{
    using System.Collections.Generic;

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
