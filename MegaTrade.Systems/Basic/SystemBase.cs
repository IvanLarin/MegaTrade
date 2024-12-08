using MegaTrade.Common;
using MegaTrade.Common.Extensions;
using MegaTrade.Common.Painting;
using MegaTrade.Common.Painting.Palettes;
using MegaTrade.Systems.Basic.Trading;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Systems.Basic;

[HandlerCategory("МегаСистемы")]
public abstract class SystemBase : IHandler, IContextUses, ITradeRules, INowProvider
{
    protected void Setup(ISecurity security)
    {
        BasicTimeframe = security;
        TradeFromBar = Context.TradeFromBar; //TODO
    }

    protected void Run()
    {
        for (var i = TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;

            if (ShouldEnterLong)
            {
                Trade.EnterLongAtMarket(LongEnterVolume);
                LongEnterSignals[Now] = true;
            }

            if (ShouldExitLong)
            {
                Trade.ExitLongAtMarket(LongExitVolume);
                LongExitSignals[Now] = true;
            }

            if (ShouldEnterShort)
            {
                Trade.EnterShortAtMarket(ShortEnterVolume);
                ShortEnterSignals[Now] = true;
            }

            if (ShouldExitShort)
            {
                Trade.ExitShortAtMarket(ShortExitVolume);
                ShortExitSignals[Now] = true;
            }
        }

        if (!Context.IsOptimization)
            DoDraw();
    }

    public int Now { get; private set; }

    protected virtual bool IsLongEnterSignal => false;

    protected virtual bool IsLongExitSignal => false;

    protected virtual bool IsShortEnterSignal => false;

    protected virtual bool IsShortExitSignal => false;

    protected virtual double LongEnterVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double LongExitVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double ShortEnterVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected virtual double ShortExitVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    private bool ShouldEnterLong =>
        IsLongTrade && IsLongEnterSignal &&
        !IsLastCandleOfSession && !IsJustBeforeLastCandleOfSession &&
        LongEnterVolume.IsMoreThan(0);

    private bool ShouldEnterShort =>
        IsShortTrade && IsShortEnterSignal &&
        !IsLastCandleOfSession && !IsJustBeforeLastCandleOfSession &&
        ShortEnterVolume.IsMoreThan(0);

    private bool ShouldExitLong =>
        IsLongTrade && IsLongExitSignal &&
        !IsLastCandleOfSession &&
        LongExitVolume.IsMoreThan(0);

    protected bool ShouldExitShort =>
        IsShortTrade && IsShortExitSignal &&
        !IsLastCandleOfSession &&
        ShortExitVolume.IsMoreThan(0);

    protected bool InLongPosition => Trade.InLongPosition;

    protected bool NotInLongPosition => !InLongPosition;

    protected bool InShortPosition => Trade.InShortPosition;

    protected bool NotInShortPosition => !InShortPosition;

    protected int? LongEnterBarNumber => Trade.LongEnterBarNumber;

    protected int? ShortEnterBarNumber => Trade.ShortEnterBarNumber;

    protected int TradeFromBar { get; set; }

    private AntiGap? _antiGap;

    private AntiGap AntiGap => _antiGap ??= new AntiGap
    {
        Context = Context,
        Security = BasicTimeframe
    };

    private bool IsLastCandleOfSession => AntiGap.IsLastCandleOfSession(Now);

    private bool IsJustBeforeLastCandleOfSession => AntiGap.IsJustBeforeLastCandleOfSession(Now);

    private bool[]? _longEnterSignals; //TODO

    private bool[]? _longExitSignals;

    private bool[]? _shortEnterSignals;

    private bool[]? _shortExitSignals;

    private bool[] LongEnterSignals => _longEnterSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private bool[] LongExitSignals => _longExitSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private bool[] ShortEnterSignals => _shortEnterSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private bool[] ShortExitSignals => _shortExitSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private IPaint? _paint;

    protected IPaint Paint => _paint ??= AddPaint(BasicTimeframe.Symbol).DecimalPlaces(BasicTimeframe.Decimals);

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
        //TODO
        Paint.Signal(Select(() => IsLastCandleOfSession || IsJustBeforeLastCandleOfSession), "Антигэп");

        if (IsBasicTimeframeDraw)
            Paint.Candles(BasicTimeframe, BasicTimeframe.Symbol).DecimalPlaces(BasicTimeframe.Decimals);

        if (IsLongTrade)
        {
            Paint.Signal(LongEnterSignals, "Сигнал на вход в лонг", AnimalColor.Bull);
            Paint.Signal(LongExitSignals, "Сигнал на выход из лонга", AnimalColor.Bear);
        }

        if (IsShortTrade)
        {
            Paint.Signal(ShortEnterSignals, "Сигнал на вход в шорт", AnimalColor.Bull);
            Paint.Signal(ShortExitSignals, "Сигнал на выход из шорта", AnimalColor.Bear);
        }

        Draw();

        Paint.Trades(BasicTimeframe);
    }

    protected IList<T> Select<T>(Func<T> func) where T : struct
    {
        T[] result = Context.GetOrCreateArray<T>(Context.BarsCount);

        for (var i = TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;
            result[i] = func();
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

    protected ISecurity BasicTimeframe
    {
        get => _security ?? throw new Exception("Забыли вызвать метод Setup");
        private set => _security = value;
    }

    private ITrade? _trade;

    private ITrade Trade => _trade ??= Context.Runtime.IsRealTime
        ? new RealTrade
        {
            BasicTimeframe = BasicTimeframe,
            TradeRules = this,
            NowProvider = this
        }
        : new HistoryTrade
        {
            BasicTimeframe = BasicTimeframe,
            TradeRules = this,
            NowProvider = this
        };

    [Description("Открывать ли длинные позиции")]
    [HandlerParameter(true, "true")]
    public bool IsLongTrade { get; set; }

    [Description("Открывать ли короткие позиции")]
    [HandlerParameter(true, "true")]
    public bool IsShortTrade { get; set; }
}