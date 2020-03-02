namespace AppInsights.Http.Internal.Metadata
{
    using System.Collections.Generic;
    using AppInsights.Http.Metadata;

    internal class Metadata : IMetadata
    {
        internal Metadata(IReadOnlyCollection<IMetadataMetric> metrics, IReadOnlyCollection<IMetadataDimension> dimensions)
        {
            Metrics = metrics;
            Dimensions = dimensions;
        }

        public IReadOnlyCollection<IMetadataMetric> Metrics { get; }
        public IReadOnlyCollection<IMetadataDimension> Dimensions { get; }
    }
}
