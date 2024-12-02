using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal interface IPaintCandles
{
    void Candles(ISecurity security, string name);

    void Trades(ISecurity security);
}