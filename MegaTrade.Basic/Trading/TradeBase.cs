using MegaTrade.Basic.Gapping;
using MegaTrade.Basic.Trading.Position;
using MegaTrade.Common.Extensions;
using TSLab.Script;

namespace MegaTrade.Basic.Trading;

internal abstract class TradeBase : ITrade
{
    public void Do()
    {
        if (IsLongEnter)
            EnterLongAtMarket();

        if (IsLongExit)
            ExitLongAtMarket();

        if (IsShortEnter)
            EnterShortAtMarket();

        if (IsShortExit)
            ExitShortAtMarket();

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
        {
            BasicTimeframe.Positions.BuyAtMarket(OnTheNextCandle, LongEnterLots, PositionNames.LongEnterName);
            UpdateLongPosition();
        }
        else
            LongPosition.ChangeAtMarket(OnTheNextCandle, lotsInPosition + LongEnterLots,
                PositionNames.LongIncreaseName);
    }

    private void ExitLongAtMarket()
    {
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsLessOrEqualTo(LongExitLots))
            LongPosition.CloseAtMarket(OnTheNextCandle, PositionNames.LongExitName);
        else
            LongPosition.ChangeAtMarket(OnTheNextCandle, lotsInPosition - LongExitLots,
                PositionNames.LongDecreaseName);
    }

    private void EnterShortAtMarket()
    {
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(0))
        {
            BasicTimeframe.Positions.SellAtMarket(OnTheNextCandle, ShortEnterLots, PositionNames.ShortEnterName);
            UpdateShortPosition();
        }
        else
            ShortPosition.ChangeAtMarket(OnTheNextCandle, lotsInPosition + ShortEnterLots,
                PositionNames.ShortIncreaseName);
    }

    private void ExitShortAtMarket()
    {
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsLessOrEqualTo(ShortExitLots))
            ShortPosition.CloseAtMarket(OnTheNextCandle, PositionNames.ShortExitName);
        else
            ShortPosition.ChangeAtMarket(OnTheNextCandle, lotsInPosition - ShortExitLots,
                PositionNames.ShortDecreaseName);
    }

    private double LongEnterLots => ToLotsCount(Signals.LongEnterVolume);

    private double LongExitLots => ToLotsCount(Signals.LongExitVolume);

    private double ShortEnterLots => ToLotsCount(Signals.ShortEnterVolume);

    private double ShortExitLots => ToLotsCount(Signals.ShortExitVolume);

    private void DoLongStops()
    {
        if (TradeRules.IsLongTrade && LongPosition.IsActive && !AntiGap.IsLastCandleOfSession)
        {
            if (Signals.LongTake.HasValue)
                LongPosition.CloseAtProfit(OnTheNextCandle, Signals.LongTake.Value,
                    PositionNames.LongTakeProfit);

            if (Signals.LongStop.HasValue)
                LongPosition.CloseAtStop(OnTheNextCandle, Signals.LongStop.Value,
                    PositionNames.LongStopLoss);
        }
    }

    private void DoShortStops()
    {
        if (TradeRules.IsShortTrade && ShortPosition.IsActive && !AntiGap.IsLastCandleOfSession)
        {
            if (Signals.ShortTake.HasValue)
                ShortPosition.CloseAtProfit(OnTheNextCandle, Signals.ShortTake.Value,
                    PositionNames.ShortTakeProfit);

            if (Signals.ShortStop.HasValue)
                ShortPosition.CloseAtStop(OnTheNextCandle, Signals.ShortStop.Value,
                    PositionNames.ShortStopLoss);
        }
    }

    private double LotsInLongPosition => LongPosition.Shares;

    private double LotsInShortPosition => ShortPosition.Shares;

    protected abstract void UpdateLongPosition();

    protected abstract void UpdateShortPosition();

    protected abstract IMarketPosition LongPosition { get; }

    protected abstract IMarketPosition ShortPosition { get; }

    public IPositionInfo LongPositionInfo => LongPosition;

    public IPositionInfo ShortPositionInfo => ShortPosition;

    public bool InLongPosition => LongPosition.IsOpen;

    public bool InShortPosition => ShortPosition.IsOpen;

    private double ToLotsCount(double volume) => BasicTimeframe.RoundShares(volume / BasicTimeframe.LotSize);

    public required ISecurity BasicTimeframe { get; init; }

    public required ITradeRules TradeRules { get; init; }

    public required INowProvider NowProvider { get; init; }

    protected int Now => NowProvider.Now;

    protected int OnTheNextCandle => Now + 1;

    public required IAntiGap AntiGap { get; init; }

    public required ISignals Signals { get; init; }
}