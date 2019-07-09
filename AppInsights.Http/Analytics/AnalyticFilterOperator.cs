using System;
using System.Collections.Generic;
using System.Text;

namespace AppInsights.Http.Analytics
{
    public class AnalyticFilterOperator
    {
        public static AnalyticFilterOperator Contains = new AnalyticFilterOperator("contains");
        public static AnalyticFilterOperator ContainsCs = new AnalyticFilterOperator("containscs");
        public static AnalyticFilterOperator EndsWith = new AnalyticFilterOperator("endswith");
        public static AnalyticFilterOperator Equal = new AnalyticFilterOperator("==");
        public static AnalyticFilterOperator Has = new AnalyticFilterOperator("has");
        public static AnalyticFilterOperator HasPrefix = new AnalyticFilterOperator("hasprefix");
        public static AnalyticFilterOperator HasSuffix = new AnalyticFilterOperator("hassuffix");
        public static AnalyticFilterOperator Like = new AnalyticFilterOperator("=~");
        public static AnalyticFilterOperator In = new AnalyticFilterOperator("in");
        public static AnalyticFilterOperator MatchesRegex = new AnalyticFilterOperator("matches regex");
        public static AnalyticFilterOperator NotContains = new AnalyticFilterOperator("!contains");
        public static AnalyticFilterOperator NotContainsCs = new AnalyticFilterOperator("!containscs");
        public static AnalyticFilterOperator NotEndsWith = new AnalyticFilterOperator("!endswith");
        public static AnalyticFilterOperator NotEqual = new AnalyticFilterOperator("!=");
        public static AnalyticFilterOperator NotHas = new AnalyticFilterOperator("!has");
        public static AnalyticFilterOperator NotHasPrefix = new AnalyticFilterOperator("!hasprefix");
        public static AnalyticFilterOperator NotHasSuffix = new AnalyticFilterOperator("!hassuffix");
        public static AnalyticFilterOperator NotLike = new AnalyticFilterOperator("!~");
        public static AnalyticFilterOperator NotIn = new AnalyticFilterOperator("!in");
        public static AnalyticFilterOperator NotStartsWith = new AnalyticFilterOperator("!startswith");
        public static AnalyticFilterOperator StartsWith = new AnalyticFilterOperator("startswith");


        private readonly string _filterOperator;
        private AnalyticFilterOperator(string filterOperator) => _filterOperator = filterOperator;

        public override string ToString()
        {
            return _filterOperator;
        }
    }
}
