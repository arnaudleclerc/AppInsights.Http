public class AnalyticTopOrderingOperator
{
    public static AnalyticTopOrderingOperator Asc = new AnalyticTopOrderingOperator("asc");
    public static AnalyticTopOrderingOperator Desc = new AnalyticTopOrderingOperator("desc");

    private readonly string _topOperator;

    private AnalyticTopOrderingOperator(string topOperator) => _topOperator = topOperator;

    public override string ToString() => _topOperator;
}