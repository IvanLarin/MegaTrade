using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal interface ITrade : ITradeSignals
{
    void Do();

    IPositionInfo LongPositionInfo { get; }

    IPositionInfo ShortPositionInfo { get; }

    bool InLongPosition { get; }

    bool InShortPosition { get; }
}

