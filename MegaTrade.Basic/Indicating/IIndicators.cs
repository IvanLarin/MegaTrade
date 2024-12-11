using TSLab.Script;

namespace MegaTrade.Basic.Indicating;

public interface IIndicators
{
    IList<double> MACD(IList<double> source, int smallPeriod, int bigPeriod);

    IList<double> RSI(IList<double> source, int period);

    IList<double> ATR(ISecurity source, int period);
}