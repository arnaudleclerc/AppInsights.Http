using AppInsights.Http.Analytics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AnalyticsQueryBuilder : IAnalyticsQueryFilterBuilder
    {
        private readonly List<string> _filters = new List<string>();

        public AnalyticsSchema Schema { get; }

        public AnalyticsQueryBuilder(AnalyticsSchema schema) => Schema = schema;

        public IAnalyticsQueryFilterBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, string value)
        {
            if (filterOperator == AnalyticFilterOperator.In || filterOperator == AnalyticFilterOperator.NotIn)
            {
                return WithFilter(filterName, filterOperator, new[] { value });
            }
            _filters.Add($"| where {filterName} {filterOperator.ToString()} '{value}'");
            return this;
        }

        public IAnalyticsQueryFilterBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, params string[] values)
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
            _filters.Add($"| where {filterName} {filterOperator.ToString()} ({filter})");
            return this;
        }

        public IAnalyticsQueryFilterBuilder WithSummarizeCount(params string[] fields)
        {
            _filters.Add($"| summarize count() by {string.Join(", ", fields)}");
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder(Schema.ToString());
            if (_filters?.Count > 0)
            {
                foreach (var filter in _filters)
                {
                    builder.Append(" " + filter);
                }
            }
            return builder.ToString();
        }
    }
}
