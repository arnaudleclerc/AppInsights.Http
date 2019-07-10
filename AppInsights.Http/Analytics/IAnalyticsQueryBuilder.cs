namespace AppInsights.Http.Analytics
{
    public interface IAnalyticsQueryFilterBuilder: IAnalyticsQueryBuilder
    {
        IAnalyticsQueryFilterBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, string value);
        IAnalyticsQueryFilterBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, params string[] value);
        IAnalyticsQueryFilterBuilder WithSummarizeCount(params string[] fields);
    }

    public interface IAnalyticsQueryBuilder
    {
        AnalyticsSchema Schema { get; }
    }
}
