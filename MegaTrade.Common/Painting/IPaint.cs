using TSLab.Script;

namespace MegaTrade.Common.Painting;

public interface IPaint
{
    void Candles(ISecurity security, string name);

    void Trades(ISecurity security);
}