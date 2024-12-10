using MegaTrade.Basic.Gapping;
using MegaTrade.Common.Extensions;
using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal abstract class TradeBase : ITrade
{
    public void Do()
    {
        UpdateNow();

        if (IsLongEnter)
            EnterLongAtMarket();

        if (IsLongExit)
            ExitLongAtMarket();

        if (IsShortEnter)
            EnterShortAtMarket();

        if (IsShortExit)
            ExitShortAtMarket();

        UpdateNext();

        DoLongStops();

        DoShortStops();
    }

    public bool IsLongEnter =>
        TradeRules.IsLongTrade && Signals.IsLongEnterSignal &&
        AntiGap is { IsLastCandleOfSession: false, IsJustBeforeLastCandleOfSession: false } &&
        LongEnterLots.IsMoreThan(0);

    public bool IsLongExit =>
        TradeRules.IsLongTrade && Signals.IsLongExitSignal &&
        !AntiGap.IsLastCandleOfSession &&
        LongExitLots.IsMoreThan(0);

    public bool IsShortEnter =>
        TradeRules.IsShortTrade && Signals.IsShortEnterSignal &&
        AntiGap is { IsLastCandleOfSession: false, IsJustBeforeLastCandleOfSession: false } &&
        ShortEnterLots.IsMoreThan(0);

    public bool IsShortExit =>
        TradeRules.IsShortTrade && Signals.IsShortExitSignal &&
        !AntiGap.IsLastCandleOfSession &&
        ShortExitLots.IsMoreThan(0);

    private void EnterLongAtMarket()
    {
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsEqualTo(0))
            BasicTimeframe.Positions.BuyAtMarket(OnTheNextCandle, LongEnterLots, PositionNames.LongEnterName);
        else
            LongPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition + LongEnterLots,
                PositionNames.LongIncreaseName);
    }

    private void ExitLongAtMarket()
    {
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsLessOrEqualTo(LongExitLots))
            LongPosition?.CloseAtMarket(OnTheNextCandle, PositionNames.LongExitName);
        else
            LongPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition - LongExitLots,
                PositionNames.LongDecreaseName);
    }

    private void EnterShortAtMarket()
    {
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(0))
            BasicTimeframe.Positions.SellAtMarket(OnTheNextCandle, ShortEnterLots, PositionNames.ShortEnterName);
        else
            ShortPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition + ShortEnterLots,
                PositionNames.ShortIncreaseName);
    }

    private void ExitShortAtMarket()
    {
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsLessOrEqualTo(ShortExitLots))
            ShortPosition?.CloseAtMarket(OnTheNextCandle, PositionNames.ShortExitName);
        else
            ShortPosition?.ChangeAtMarket(OnTheNextCandle, lotsInPosition - ShortExitLots,
                PositionNames.ShortDecreaseName);
    }

    private double LongEnterLots => ToLotsCount(Signals.LongEnterVolume);

    private double LongExitLots => ToLotsCount(Signals.LongExitVolume);

    private double ShortEnterLots => ToLotsCount(Signals.ShortEnterVolume);

    private double ShortExitLots => ToLotsCount(Signals.ShortExitVolume);

    private void DoLongStops()
    {
        if (TradeRules.IsLongTrade && NextLongPosition != null && !AntiGap.IsLastCandleOfSession)
        {
            var positionInfo = new ReturningFromTheFuture(NextLongPosition)
            {
                NowProvider = NowProvider,
                BasicTimeframe = BasicTimeframe
            };

            var take = Signals.GetLongTake(positionInfo);
            var stop = Signals.GetLongStop(positionInfo);

            if (take.HasValue)
                NextLongPosition.CloseAtProfit(OnTheNextCandle, take.Value,
                    PositionNames.LongTakeProfit);

            if (stop.HasValue)
                NextLongPosition.CloseAtStop(OnTheNextCandle, stop.Value,
                    PositionNames.LongStopLoss);
        }
    }

    private void DoShortStops()
    {
        if (TradeRules.IsShortTrade && NextShortPosition != null && !AntiGap.IsLastCandleOfSession)
        {
            var positionInfo = new ReturningFromTheFuture(NextShortPosition)
            {
                NowProvider = NowProvider,
                BasicTimeframe = BasicTimeframe
            };

            var take = Signals.GetShortTake(positionInfo);
            var stop = Signals.GetShortStop(positionInfo);

            if (take.HasValue)
                NextShortPosition.CloseAtProfit(OnTheNextCandle, take.Value,
                    PositionNames.ShortTakeProfit);

            if (stop.HasValue)
                NextShortPosition.CloseAtStop(OnTheNextCandle, stop.Value,
                    PositionNames.ShortStopLoss);
        }
    }

    private double LotsInLongPosition => LongPosition?.Shares ?? 0;

    private double LotsInShortPosition => ShortPosition?.Shares ?? 0;

    protected abstract void UpdateNow();

    protected abstract void UpdateNext();

    protected abstract IPosition? LongPosition { get; }

    protected abstract IPosition? ShortPosition { get; }

    protected abstract IPosition? NextLongPosition { get; }

    protected abstract IPosition? NextShortPosition { get; }

    public IPositionInfo? LongPositionInfo => LongPosition;

    public IPositionInfo? ShortPositionInfo => ShortPosition;

    private double ToLotsCount(double volume) => BasicTimeframe.RoundShares(volume / BasicTimeframe.LotSize);

    public required ISecurity BasicTimeframe { get; init; }

    public required ITradeRules TradeRules { get; init; }

    public required INowProvider NowProvider { get; init; }

    protected int Now => NowProvider.Now;

    protected int OnTheNextCandle => Now + 1;

    public required IAntiGap AntiGap { get; init; }

    public required ISignals Signals { get; init; }
}