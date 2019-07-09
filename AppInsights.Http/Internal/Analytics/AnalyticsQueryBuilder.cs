using AppInsights.Http.Analytics;
using System.Collections.Generic;
using System.Text;

namespace AppInsights.Http.Internal.Analytics
{
    internal class AnalyticsQueryBuilder : IAnalyticsQueryBuilder
    {
        private IList<string> _filters;

        public AnalyticsSchema Schema { get; }

        public AnalyticsQueryBuilder(AnalyticsSchema schema) => Schema = schema;

        public IAnalyticsQueryFilterBuilder WithFilter(string filterName, AnalyticFilterOperator filterOperator, string value)
        {
            if (_filters == null)
            {
                _filters = new List<string>();
            }
            _filters.Add($"| where {filterName} {filterOperator.ToString()} {value}");
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
