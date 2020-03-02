using System.Collections.Generic;
using System.Text;
using AppInsights.Http.Analytics;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AnalyticsQueryBuilder : IAnalyticsQueryBuilder
    {
        private readonly List<string> _filters = new List<string>();
        private string _project;

        public AnalyticsSchema Schema { get; }

        public AnalyticsQueryBuilder(AnalyticsSchema schema) => Schema = schema;

        public IAnalyticsQueryBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, string value)
        {
            if (filterOperator == AnalyticFilterOperator.In || filterOperator == AnalyticFilterOperator.NotIn)
            {
                return WithFilter(filterName, filterOperator, new [] { value });
            }
            _filters.Add($"| where {filterName} {filterOperator} '{value}'");
            return this;
        }

        public IAnalyticsQueryBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, params string[] values)
        {
            var filter = string.Empty;
            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    filter = $"'{value}'";
                }
                else
                {
                    filter += $", '{value}'";
                }
            }
            _filters.Add($"| where {filterName} {filterOperator} ({filter})");
            return this;
        }

        public IAnalyticsQueryBuilder WithSummarizeCount(params string[] fields)
        {
            _filters.Add($"| summarize count() by {string.Join(", ", fields)}");
            return this;
        }

        public IAnalyticsQueryBuilder WithProject(params string[] fields)
        {
            _project = $"| project {string.Join(", ", fields)}";
            return this;
        }

        public IAnalyticsQueryBuilder WithTimestampFilter(AnalyticFilterOperator filterOperator, AnalyticTimestampFilterOperator timestampFilterOperator, AnalyticTimestampDuration duration)
        {
            _filters.Add($"| where timestamp {filterOperator} {timestampFilterOperator}({duration})");
            return this;
        }

        public IAnalyticsQueryBuilder WithTimestampFilter(AnalyticFilterOperator filterOperator, AnalyticTimestampFilterOperator timestampFilterOperator, string duration)
        {
            _filters.Add($"| where timestamp {filterOperator} {timestampFilterOperator}({duration})");
            return this;
        }

        public IAnalyticsQueryBuilder WithTop(int top, string byField)
        {
            _filters.Add($"| top {top} by {byField}");
            return this;
        }

        public IAnalyticsQueryBuilder WithTop(int top, string byField, AnalyticTopOrderingOperator topOrderingOperator)
        {
            _filters.Add($"| top {top} by {byField} {topOrderingOperator}");
            return this;
        }

        public IAnalyticsQueryBuilder WithTop(int top, string byField, AnalyticNullsOrderingOperator nullsOrderingOperator)
        {
             _filters.Add($"| top {top} by {byField} {nullsOrderingOperator}");
            return this;
        }

        public IAnalyticsQueryBuilder WithTop(int top, string byField, AnalyticTopOrderingOperator topOrderingOperator, AnalyticNullsOrderingOperator nullsOrderingOperator)
        {
            _filters.Add($"| top {top} by {byField} {topOrderingOperator} {nullsOrderingOperator}");
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Schema.ToString());
            if (_filters.Count > 0)
            {
                foreach (var filter in _filters)
                {
                    builder.Append(" " + filter);
                }
            }

            if (!string.IsNullOrWhiteSpace(_project))
            {
                builder.Append(" " + _project);
            }

            return builder.ToString();
        }
    }
}