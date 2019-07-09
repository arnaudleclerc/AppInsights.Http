using System.Collections.Generic;

namespace AppInsights.Http.Metadata
{
    public interface IMetadata
    {
        IReadOnlyCollection<IMetadataMetric> Metrics { get; }
        IReadOnlyCollection<IMetadataDimension> Dimensions { get; }
    }
}
