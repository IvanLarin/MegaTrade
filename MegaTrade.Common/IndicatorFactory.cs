namespace MegaTrade.Common;

public interface IndicatorFactory
{
    IList<double> MakeMacd(IList<double> source, int smallPeriod, int bigPeriod);

    IList<double> MakeRsi(IList<double> source, int period);
}