using System.Collections.Generic;

namespace AppInsights.Http.Analytics.Internals
{
    internal class AnalyticQueryResultRow : IAnalyticQueryResultRow
    {
        public IReadOnlyDictionary<string, string> Columns { get; }

        public AnalyticQueryResultRow(IReadOnlyDictionary<string, string> columns) => Columns = columns;
    }
}
