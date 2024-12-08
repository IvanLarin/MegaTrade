namespace MegaTrade.Systems.Basic.Trading;

internal interface ITrade
{
    void EnterLongAtMarket(double volume);

    void EnterShortAtMarket(double volume);

    void ExitLongAtMarket(double volume);

    void ExitShortAtMarket(double volume);

    bool InLongPosition { get; }

    bool InShortPosition { get; }

    int? LongEnterBarNumber { get; }

    int? ShortEnterBarNumber { get; }
}