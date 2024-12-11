using TSLab.Script;

namespace MegaTrade.Basic.Trading.Position;

public interface IMarketPosition : IPosition
{
    bool IsOpen { get; }
}