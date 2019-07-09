namespace AppInsights.Http
{
    public interface IMetadataDimension
    {
        string Metric { get; }
        string DisplayName { get; }
    }
}
