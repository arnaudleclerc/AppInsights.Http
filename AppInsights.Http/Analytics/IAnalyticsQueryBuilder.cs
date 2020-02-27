namespace AppInsights.Http.Analytics
{
    /// <summary>
    /// Exposes methods to build the query to send to application insights
    /// </summary>
    public interface IAnalyticsQueryBuilder
    {
        /// <summary>
        /// Schema on which the query will be built
        /// </summary>
        /// <value></value>
        AnalyticsSchema Schema { get; }

        /// <summary>
        /// Add a where clause on the query with the given parameter with the format "| where {filterName} {filterOperator} '{value}'"
        /// </summary>
        /// <param name="filterName">Name of the field on which the filter will be applied</param>
        /// <param name="filterOperator">Operator to filter with</param>
        /// <param name="value">Value of the filter</param>
        /// <returns>Query builder with the filter applied</returns>
        IAnalyticsQueryBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, string value);
        /// <summary>
        /// Add a where clause on the query with the given parameter, separating the value on the filter with a comma.
        /// "| where {filterName} {filterOperator} '{value[0]}', '{value[1]}, ...{[value[n]]}"
        /// </summary>
        /// <param name="filterName">Name of the field on which the filter will be applied</param>
        /// <param name="filterOperator">Operator to filter with</param>
        /// <param name="value">Value of the filter</param>
        /// <returns>Query builder with the filter applied</returns>
        IAnalyticsQueryBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, params string[] value);
        /// <summary>
        /// Add a where clause with a filter on the timestamp field with the format "|where timestamp {filterOperator} {timestampFilterOperator}({duration})"
        /// </summary>
        /// <param name="filterOperator">Operator to filter with</param>
        /// <param name="timestampFilterOperator">Method to run on the timestamp filter</param>
        /// <param name="duration">Predefined duration</param>
        /// <returns>Query builder with the timestamp filter applied</returns>
        IAnalyticsQueryBuilder WithTimestampFilter(AnalyticFilterOperator filterOperator, AnalyticTimestampFilterOperator timestampFilterOperator, AnalyticTimestampDuration duration);
        /// <summary>
        /// Add a where clause with a filter on the timestamp field with the format "|where timestamp {filterOperator} {timestampFilterOperator}({duration})"
        /// </summary>
        /// <param name="filterOperator">Operator to filter with</param>
        /// <param name="timestampFilterOperator">Method to run on the timestamp filter</param>
        /// <param name="duration">Duration</param>
        /// <returns>Query builder with the timestamp filter applied</returns>
        IAnalyticsQueryBuilder WithTimestampFilter(AnalyticFilterOperator filterOperator, AnalyticTimestampFilterOperator timestampFilterOperator, string duration);
        /// <summary>
        /// Add a summarize by count clause on the query with the format "|summarize count() by {fields[0]}, {fields[1]},...{fields[n]}"
        /// </summary>
        /// <param name="fields">Fields to add to the clause</param>
        /// <returns>Query builder with the summarite by count clause applied</returns>
        IAnalyticsQueryBuilder WithSummarizeCount(params string[] fields);
        /// <summary>
        /// Add a project clause on the query with the format "|project {fields[0]}, {fields[1]},...{fields[n]}" 
        /// </summary>
        /// <param name="fields">Fields to add to the clause</param>
        /// <returns>Query builder with the project clause applied</returns>
        IAnalyticsQueryBuilder WithProject(params string[] fields);
        /// <summary>
        /// Add a top clause on the query with the format "|top {top} by {byField}"
        /// </summary>
        /// <param name="top">Amount of entries to retrieve</param>
        /// <param name="byField">Field to apply on the top clause</param>
        /// <returns>Query builder with the top clause applied</returns>
        IAnalyticsQueryBuilder WithTop(int top, string byField);
        /// <summary>
        /// Add a top clause on the query with the format "|top {top} by {byField} {topOrderingOperator}"
        /// </summary>
        /// <param name="top">Amount of entries to retrieve</param>
        /// <param name="byField">Field to apply to the top clause</param>
        /// <param name="topOrderingOperator">Order to apply to the top clause</param>
        /// <returns>Query builder with the top clause applied</returns>
        IAnalyticsQueryBuilder WithTop(int top, string byField, AnalyticTopOrderingOperator topOrderingOperator);
        /// <summary>
        /// Add a top clause on the query with the format "|top {top} by {byField} {nullsOrderingOperator}"
        /// </summary>
        /// <param name="top">Amount of entries to retrieve</param>
        /// <param name="byField">Field to apply to the top clause</param>
        /// <param name="nullsOrderingOperator">Position of the null values on the top clause</params>
        /// <returns>Query builder with the top clause applied</returns>
        IAnalyticsQueryBuilder WithTop(int top, string byField, AnalyticNullsOrderingOperator nullsOrderingOperator);
        /// <summary>
        /// Add a top clause on the query with the format "|top {top} by {byField} {topOrderingOperator} {nullsOrderingOperator}"
        /// </summary>
        /// <param name="top">Amount of entries to retrieve</param>
        /// <param name="byField">Field to apply to the top clause</param>
        /// <param name="topOrderingOperator">Order to apply to the top clause</param>
        /// <param name="nullsOrderingOperator">Position of the null values on the top clause</params>
        /// <returns>Query builder with the top clause applied</returns>
        IAnalyticsQueryBuilder WithTop(int top, string byField, AnalyticTopOrderingOperator topOrderingOperator, AnalyticNullsOrderingOperator nullsOrderingOperator);
    }
}