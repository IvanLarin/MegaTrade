namespace MegaTrade.Common.Calculating;

internal class CalculatePercentile : Calculator<double>
{
    protected override void CheckInput()
    {
        if (Values.Length == 0)
            throw new ArgumentException("Список не должен быть пустым");

        Percentile = Math.Min(100, Percentile);
        Percentile = Math.Max(0, Percentile);
    }

    protected override double Calculate()
    {
        List<double> sortedValues = Values.OrderBy(v => v).ToList();
        var index = (int)Math.Ceiling(Percentile / 100 * sortedValues.Count) - 1;
        return sortedValues[index];
    }

    public required double[] Values { private get; set; }

    public required double Percentile { private get; set; }
}