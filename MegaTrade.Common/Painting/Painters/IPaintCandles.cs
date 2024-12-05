using TSLab.Script;

namespace MegaTrade.Common.Painting.Painters;

internal interface IPaintCandles
{
    void Candles(ISecurity security, string? name = null);

    void Trades(ISecurity security);
}