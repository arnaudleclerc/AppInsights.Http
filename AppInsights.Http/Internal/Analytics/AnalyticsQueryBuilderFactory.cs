using AppInsights.Http.Analytics;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AnalyticsQueryBuilderFactory : IAnalyticsQueryBuilderFactory
    {
        public IAnalyticsQueryFilterBuilder CreateAnalyticsQueryBuilder(AnalyticsSchema schema)
        {
            return new AnalyticsQueryBuilder(schema);
        }
    }
}
