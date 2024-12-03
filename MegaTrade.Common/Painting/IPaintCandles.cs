using TSLab.Script;

namespace MegaTrade.Common.Painting;

public interface IPaintCandles
{
    void Candles(ISecurity security, string? name = null);

    void Trades(ISecurity security);
}