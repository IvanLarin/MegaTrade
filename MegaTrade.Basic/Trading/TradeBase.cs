using MegaTrade.Basic.Gapping;
using MegaTrade.Common.Extensions;
using TSLab.Script;

namespace MegaTrade.Basic.Trading;

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
            BasicTimeframe.Positions.BuyAtMarket(OnTheNextCandle, lots, PositionNames.LongEnterName);
            UpdateLongPosition();
        }
        else
        {
            LongPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition + lots, PositionNames.LongIncreaseName);
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
            BasicTimeframe.Positions.SellAtMarket(OnTheNextCandle, lots, PositionNames.ShortEnterName);
            UpdateShortPosition();
        }
        else
        {
            ShortPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition + lots, PositionNames.ShortIncreaseName);
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
            LongPosition?.CloseAtMarket(OnTheNextCandle, PositionNames.LongExitName);
            UpdateLongPosition();
        }
        else
        {
            LongPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition - lots, PositionNames.LongDecreaseName);
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
            ShortPosition?.CloseAtMarket(OnTheNextCandle, PositionNames.ShortExitName);
            UpdateShortPosition();
        }
        else
        {
            ShortPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition - lots, PositionNames.ShortDecreaseName);
        }
    }

    public void Do()
    {
        DoLongStops();
        DoShortStops();
    }

    private void DoLongStops()
    {
        if (TradeRules.IsLongTrade && LongPosition != null && !AntiGap.IsLastCandleOfSession)
        {
            if (Stops.LongTake.HasValue)
                LongPosition.CloseAtProfit(OnTheNextCandle, Stops.LongTake.Value,
                    PositionNames.LongTakeProfit);

            if (Stops.LongStop.HasValue)
                LongPosition.CloseAtStop(OnTheNextCandle, Stops.LongStop.Value,
                    PositionNames.LongStopLoss);
        }
    }

    private void DoShortStops()
    {
        if (TradeRules.IsShortTrade && ShortPosition != null && !AntiGap.IsLastCandleOfSession)
        {
            if (Stops.ShortTake.HasValue)
                ShortPosition.CloseAtProfit(OnTheNextCandle, Stops.ShortTake.Value,
                    PositionNames.ShortTakeProfit);

            if (Stops.ShortStop.HasValue)
                ShortPosition.CloseAtStop(OnTheNextCandle, Stops.ShortStop.Value,
                    PositionNames.ShortStopLoss);
        }
    }

    public void Update()
    {
        if (TradeRules.IsLongTrade && LongPosition != null)
            UpdateLongPosition();

        if (TradeRules.IsShortTrade && ShortPosition != null)
            UpdateShortPosition();
    }

    protected abstract void UpdateLongPosition();

    protected abstract void UpdateShortPosition();

    private double LotsInLongPosition => LongPosition?.Shares ?? 0;

    private double LotsInShortPosition => ShortPosition?.Shares ?? 0;

    protected abstract IPosition? LongPosition { get; }

    protected abstract IPosition? ShortPosition { get; }

    public IPositionInfo? LongPositionInfo => LongPosition == null
        ? null
        : new ReturningFromTheFuture
        {
            PositionInfo = LongPosition,
            NowProvider = NowProvider,
            BasicTimeframe = BasicTimeframe
        };

    public IPositionInfo? ShortPositionInfo => ShortPosition == null
        ? null
        : new ReturningFromTheFuture
        {
            PositionInfo = ShortPosition,
            NowProvider = NowProvider,
            BasicTimeframe = BasicTimeframe
        };

    private double ToLotsCount(double volume) => BasicTimeframe.RoundShares(volume / BasicTimeframe.LotSize);

    public required ISecurity BasicTimeframe { get; init; }

    public required ITradeRules TradeRules { get; init; }

    public required INowProvider NowProvider { get; init; }

    protected int Now => NowProvider.Now;

    private int OnTheNextCandle => Now + 1;

    public required IAntiGap AntiGap { get; init; }

    public required IStops Stops { get; init; }
}