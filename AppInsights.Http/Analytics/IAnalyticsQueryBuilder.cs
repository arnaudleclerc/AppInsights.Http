namespace AppInsights.Http.Analytics
{
    public interface IAnalyticsQueryFilterBuilder
    {
        IAnalyticsQueryFilterBuilder WithFilter(string fiterName, AnalyticFilterOperator filterOperator, string value);
    }

    public interface IAnalyticsQueryBuilder : IAnalyticsQueryFilterBuilder
    {
        AnalyticsSchema Schema { get; }
    }
}
