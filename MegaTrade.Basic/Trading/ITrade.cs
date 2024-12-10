using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal interface ITrade
{
    void Do();

    IPositionInfo? LongPositionInfo { get; }

    IPositionInfo? ShortPositionInfo { get; }

    bool IsLongEnter { get; }

    bool IsLongExit { get; }

    bool IsShortEnter { get; }

    bool IsShortExit { get; }
}