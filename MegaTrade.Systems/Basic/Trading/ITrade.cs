using TSLab.Script;

namespace MegaTrade.Systems.Basic.Trading;

internal interface ITrade
{
    void EnterLongAtMarket(double volume);

    void EnterShortAtMarket(double volume);

    void ExitLongAtMarket(double volume);

    void ExitShortAtMarket(double volume);

    public IPositionInfo? LongPositionInfo { get; }

    public IPositionInfo? ShortPositionInfo { get; }

    void Do();
}