namespace AppInsights.Http.Analytics
{
    public interface IAnalyticsQueryBuilderFactory
    {
        IAnalyticsQueryFilterBuilder CreateAnalyticsQueryBuilder(AnalyticsSchema schema);
    }
}
