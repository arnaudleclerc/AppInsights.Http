using System;

namespace AppInsights.Http.Exceptions
{
    public sealed class AppInsightsException : Exception
    {
        internal AppInsightsException(string message, string code) : base(message)
        {
            Data.Add("AppInsightsException:Code", code);
        }
    }
}
