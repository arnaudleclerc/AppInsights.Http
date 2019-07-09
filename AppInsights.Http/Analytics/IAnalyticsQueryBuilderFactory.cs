namespace AppInsights.Http.Analytics
{
    public interface IAnalyticsQueryBuilderFactory
    {
        IAnalyticsQueryBuilder CreateAnalyticsQueryBuilder(AnalyticsSchema schema);
    }
}
