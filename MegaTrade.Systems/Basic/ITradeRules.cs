namespace MegaTrade.Systems.Basic;

internal interface ITradeRules
{
    bool IsLongTrade { get; }

    bool IsShortTrade { get; }
}