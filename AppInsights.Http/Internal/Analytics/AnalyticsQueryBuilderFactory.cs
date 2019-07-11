using AppInsights.Http.Analytics;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AnalyticsQueryBuilderFactory : IAnalyticsQueryBuilderFactory
    {
        public IAnalyticsQueryBuilder CreateAnalyticsQueryBuilder(AnalyticsSchema schema)
        {
            return new AnalyticsQueryBuilder(schema);
        }
    }
}
