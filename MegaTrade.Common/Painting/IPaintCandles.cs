using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal interface IPaintCandles
{
    void Candles(ISecurity security, string? name = null);

    void Trades(ISecurity security);
}