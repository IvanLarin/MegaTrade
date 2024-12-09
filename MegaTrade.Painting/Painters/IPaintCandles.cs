using TSLab.Script;

namespace MegaTrade.Draw.Painters;

internal interface IPaintCandles
{
    void Candles(ISecurity security, string? name = null);

    void Trades(ISecurity security);
}