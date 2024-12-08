using MegaTrade.Common.Extensions;
using TSLab.Script;

namespace MegaTrade.Systems.Basic.Trading;

internal abstract class TradeBase : ITrade
{
    public void EnterLongAtMarket(double volume)
    {
        if (!TradeRules.IsLongTrade)
            return;

        var lots = ToLotsCount(volume);

        if (lots.IsEqualTo(0))
            return;

        DoEnterLongAtMarket(lots);
    }

    private void DoEnterLongAtMarket(double lots)
    {
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsEqualTo(0))
        {
            BasicTimeframe.Positions.BuyAtMarket(Now + 1, lots, PositionNames.LongEnterName);
            UpdateLongPosition();
        }
        else
        {
            LongPosition?.ChangeAtMarket(Now + 1, lotsInPosition + lots, PositionNames.LongIncreaseName);
        }
    }

    public void EnterShortAtMarket(double volume)
    {
        if (!TradeRules.IsShortTrade)
            return;

        var lots = ToLotsCount(volume);

        if (lots.IsEqualTo(0))
            return;

        DoEnterShortAtMarket(lots);
    }

    private void DoEnterShortAtMarket(double lots)
    {
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(0))
        {
            BasicTimeframe.Positions.SellAtMarket(Now + 1, lots, PositionNames.ShortEnterName);
            UpdateShortPosition();
        }
        else
        {
            ShortPosition?.ChangeAtMarket(Now + 1, lotsInPosition + lots, PositionNames.ShortIncreaseName);
        }
    }

    public void ExitLongAtMarket(double volume)
    {
        if (!TradeRules.IsLongTrade)
            return;

        var lots = ToLotsCount(volume);

        if (lots.IsEqualTo(0))
            return;

        DoExitLongAtMarket(lots);
    }

    private void DoExitLongAtMarket(double lots)
    {
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsLessOrEqualTo(lots))
        {
            LongPosition?.CloseAtMarket(Now + 1, PositionNames.LongExitName);
            UpdateLongPosition();
        }
        else
        {
            LongPosition?.ChangeAtMarket(Now + 1, lotsInPosition - lots, PositionNames.LongDecreaseName);
        }
    }

    public void ExitShortAtMarket(double volume)
    {
        if (!TradeRules.IsShortTrade)
            return;

        var lots = ToLotsCount(volume);

        if (lots.IsEqualTo(0))
            return;

        DoExitShortAtMarket(lots);
    }

    private void DoExitShortAtMarket(double lots)
    {
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsLessOrEqualTo(lots))
        {
            ShortPosition?.CloseAtMarket(Now + 1, PositionNames.ShortExitName);
            UpdateShortPosition();
        }
        else
        {
            ShortPosition?.ChangeAtMarket(Now + 1, lotsInPosition - lots, PositionNames.ShortDecreaseName);
        }
    }

    protected abstract void UpdateLongPosition();

    protected abstract void UpdateShortPosition();

    private double LotsInLongPosition => LongPosition?.Shares ?? 0;

    private double LotsInShortPosition => ShortPosition?.Shares ?? 0;

    protected abstract IPosition? LongPosition { get; }

    protected abstract IPosition? ShortPosition { get; }

    private double ToLotsCount(double volume) => BasicTimeframe.RoundShares(volume / BasicTimeframe.LotSize);

    public bool InLongPosition => LongPosition != null;

    public bool InShortPosition => ShortPosition != null;

    public required ISecurity BasicTimeframe { get; init; }

    public required ITradeRules TradeRules { get; init; }

    public required INowProvider NowProvider { get; init; }

    protected int Now => NowProvider.Now;
}