﻿using MegaTrade.Common;
using MegaTrade.Common.Extensions;
using MegaTrade.Common.Painting;
using MegaTrade.Common.Painting.Palettes;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Systems.Base;

[HandlerCategory("МегаСистемы")]
public abstract class SystemBase : IHandler, IContextUses
{
    protected void Setup(ISecurity security)
    {
        Security = security;
        TradeFromBar = Context.TradeFromBar;
    }

    protected void Run()
    {
        for (var i = TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;

            if (ShouldEnterLong)
                EnterLongAtMarket();

            if (ShouldEnterShort)
                EnterShortAtMarket();

            if (ShouldExitLong)
                ExitLongAtMarket();

            if (ShouldExitShort)
                ExitShortAtMarket();
        }

        if (!Context.IsOptimization)
            DoDraw();
    }

    protected int Now { get; private set; }

    protected virtual bool IsLongEnterSignal => false;

    protected virtual bool IsLongExitSignal => false;

    protected virtual bool IsShortEnterSignal => false;

    protected virtual bool IsShortExitSignal => false;

    protected virtual double GetLongEnterVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double GetLongExitVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double GetShortEnterVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double GetShortExitVolume =>
        Security.LotSize; //TODO каким количеством торговать, если счёт слился?

    private bool ShouldEnterLong =>
        IsLongTrade && IsLongEnterSignal &&
        !IsLastCandleOfSession && !IsJustBeforeLastCandleOfSession &&
        LotsToLongEnter.IsMoreThan(0);

    private bool ShouldEnterShort =>
        IsShortTrade && IsShortEnterSignal &&
        !IsLastCandleOfSession && !IsJustBeforeLastCandleOfSession &&
        LotsToShortEnter.IsMoreThan(0);

    private bool ShouldExitLong =>
        IsLongTrade && IsLongExitSignal &&
        !IsLastCandleOfSession &&
        LotsToLongExit.IsMoreThan(0);

    protected bool ShouldExitShort =>
        IsShortTrade && IsShortExitSignal &&
        !IsLastCandleOfSession &&
        LotsToShortExit.IsMoreThan(0);

    private void EnterLongAtMarket()
    {
        var lotsToBuy = LotsToLongEnter;
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsEqualTo(0))
            Security.Positions.BuyAtMarket(Now + 1, lotsToBuy, LongEnterName);
        else
            LongPosition?.ChangeAtMarket(Now + 1, lotsInPosition + lotsToBuy, LongIncreaseName);
    }

    private void ExitLongAtMarket()
    {
        var lotsToSell = LotsToLongExit;
        var lotsInPosition = LotsInLongPosition;

        if (lotsInPosition.IsEqualTo(lotsToSell))
            LongPosition?.CloseAtMarket(Now + 1, LongExitName);
        else
            LongPosition?.ChangeAtMarket(Now + 1, lotsInPosition - lotsToSell, LongDecreaseName);
    }

    private void EnterShortAtMarket()
    {
        var lotsToSell = LotsToShortEnter;
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(0))
            Security.Positions.SellAtMarket(Now + 1, lotsToSell, ShortEnterName);
        else
            ShortPosition?.ChangeAtMarket(Now + 1, lotsInPosition + lotsToSell, ShortIncreaseName);
    }

    private void ExitShortAtMarket()
    {
        var lotsToBuy = LotsToShortExit;
        var lotsInPosition = LotsInShortPosition;

        if (lotsInPosition.IsEqualTo(lotsToBuy))
            ShortPosition?.CloseAtMarket(Now + 1, ShortExitName);
        else
            ShortPosition?.ChangeAtMarket(Now + 1, lotsInPosition - lotsToBuy, ShortDecreaseName);
    }

    private double LotsToLongEnter => IsLongTrade ? ToLotsCount(GetLongEnterVolume) : 0;

    private double LotsToLongExit =>
        IsLongTrade ? Math.Min(ToLotsCount(GetLongExitVolume), LotsInLongPosition) : 0;

    private double LotsToShortEnter => IsShortTrade ? ToLotsCount(GetShortEnterVolume) : 0;

    private double LotsToShortExit =>
        IsShortTrade ? Math.Min(ToLotsCount(GetShortExitVolume), LotsInShortPosition) : 0;

    private double ToLotsCount(double volume) => Security.RoundShares(volume / Security.LotSize);

    protected bool NotInLongPosition => LongPosition == null;

    protected bool NotInShortPosition => ShortPosition == null;

    private IPosition? LongPosition =>
        Security.Positions.GetLastLongPositionActive(Now);

    private IPosition? ShortPosition =>
        Security.Positions.GetLastShortPositionActive(Now);

    private double LotsInLongPosition => IsLongTrade ? LongPosition?.Shares ?? 0 : 0;

    private double LotsInShortPosition => IsShortTrade ? ShortPosition?.Shares ?? 0 : 0;

    protected int TradeFromBar { get; set; }

    private AntiGap? _antiGap;

    private AntiGap AntiGap => _antiGap ??= new AntiGap
    {
        Context = Context,
        Security = Security
    };

    private bool IsLastCandleOfSession => AntiGap.IsLastCandleOfSession(Now);

    private bool IsJustBeforeLastCandleOfSession => AntiGap.IsJustBeforeLastCandleOfSession(Now);

    private IPaint? _paint;

    protected IPaint Paint => _paint ??= AddPaint(Security.Symbol).DecimalPlaces(Security.Decimals);

    private readonly IPalette _neutralPalette = new NeutralPalette();

    protected IPaint AddPaint(string name) => new Paint
    {
        Context = Context,
        GraphName = name,
        NeutralPalette = _neutralPalette,
        BullPalette = new BullPalette(),
        BearPalette = new BearPalette()
    }.DecimalPlaces(2);

    protected virtual bool IsBasicTimeframeDraw => true;

    private void DoDraw()
    {
        Paint.Signal(Select(() => IsLastCandleOfSession || IsJustBeforeLastCandleOfSession), "Конец торговой сессии");

        if (IsBasicTimeframeDraw)
            Paint.Candles(Security, Security.Symbol).DecimalPlaces(Security.Decimals);

        if (IsLongTrade)
        {
            Paint.Signal(Select(() => IsLongEnterSignal), "Сигнал на вход в лонг", AnimalColor.Bull);
            Paint.Signal(Select(() => IsLongExitSignal), "Сигнал на выход из лонга", AnimalColor.Bear);
        }

        if (IsShortTrade)
        {
            Paint.Signal(Select(() => IsShortEnterSignal), "Сигнал на вход в шорт", AnimalColor.Bull);
            Paint.Signal(Select(() => IsShortExitSignal), "Сигнал на выход из шорта", AnimalColor.Bear);
        }

        Draw();

        Paint.Trades(Security);
    }

    protected IList<T> Select<T>(Func<T> func) where T : struct
    {
        List<T> result = Enumerable.Range(0, TradeFromBar).Select(_ => default(T)).ToList();

        for (var i = TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;
            result.Add(func());
        }

        return result;
    }

    protected virtual void Draw()
    {
    }

    private IIndicatorFactory? _indicatorFactory;

    protected IIndicatorFactory IIndicatorFactory => _indicatorFactory ??= new IndicatorFactory(Context);

    private IContext? _context;

    public IContext Context
    {
        get => _context!;
        set => _context = value;
    }

    private ISecurity? _security;

    protected ISecurity Security
    {
        get => _security ?? throw new Exception("Забыли вызвать метод Setup");
        private set => _security = value;
    }

    private const string LongEnterName = "LE";

    private const string ShortEnterName = "SE";

    private const string LongExitName = "LX";

    private const string ShortExitName = "SX";

    private const string LongIncreaseName = "L+";

    private const string LongDecreaseName = "L-";

    private const string ShortIncreaseName = "S+";

    private const string ShortDecreaseName = "S-";

    [Description("Открывать ли длинные позиции")]
    [HandlerParameter(true, "true")]
    public bool IsLongTrade { get; set; }

    [Description("Открывать ли короткие позиции")]
    [HandlerParameter(true, "true")]
    public bool IsShortTrade { get; set; }
}