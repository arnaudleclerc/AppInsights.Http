namespace AppInsights.Http.Internal.Analytics
{
    using AppInsights.Http.Analytics;

    internal class AnalyticsQueryBuilderFactory : IAnalyticsQueryBuilderFactory
    {
        public IAnalyticsQueryBuilder CreateAnalyticsQueryBuilder(AnalyticsSchema schema)
        {
            return new AnalyticsQueryBuilder(schema);
        }
    }
}
