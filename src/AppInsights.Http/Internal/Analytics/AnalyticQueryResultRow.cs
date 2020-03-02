namespace AppInsights.Http.Internal.Analytics
{
    using System.Collections.Generic;
    using AppInsights.Http.Analytics;

    internal class AnalyticQueryResultRow : IAnalyticQueryResultRow
    {
        public IReadOnlyDictionary<string, string> Columns { get; }

        public AnalyticQueryResultRow(IReadOnlyDictionary<string, string> columns) => Columns = columns;
    }
}
