namespace AppInsights.Http.Metadata
{
    using System.Collections.Generic;

    public interface IMetadata
    {
        IReadOnlyCollection<IMetadataMetric> Metrics { get; }
        IReadOnlyCollection<IMetadataDimension> Dimensions { get; }
    }
}
