using System.Collections.Generic;

namespace AppInsights.Http
{
    public interface IMetadata
    {
        IReadOnlyCollection<IMetadataMetric> Metrics { get; }
        IReadOnlyCollection<IMetadataDimension> Dimensions { get; }
    }
}
