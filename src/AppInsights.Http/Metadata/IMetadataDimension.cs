namespace AppInsights.Http.Metadata
{
    public interface IMetadataDimension
    {
        string Metric { get; }
        string DisplayName { get; }
    }
}
