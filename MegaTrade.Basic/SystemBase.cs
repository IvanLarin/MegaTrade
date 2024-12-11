using MegaTrade.Basic.Gapping;
using MegaTrade.Basic.Trading;
using MegaTrade.Common;
using MegaTrade.Common.Extensions;
using MegaTrade.Draw;
using MegaTrade.Draw.Palettes;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Basic;

[HandlerCategory("МегаСистемы")]
public abstract class SystemBase : IHandler, IContextUses, ITradeRules, INowProvider, ISignals
{
    protected void Setup(ISecurity security)
    {
        BasicTimeframe = security;
        TradeFromBar = Context.TradeFromBar; //TODO
    }

    //TODO вынести всё рисовалово в рисовалово

    protected void Run()
    {
        for (var i = TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;

            Trade.Do();

            LongEnters[Now] = Trade.IsLongEnter;
            LongExits[Now] = Trade.IsLongExit;
            ShortEnters[Now] = Trade.IsShortEnter;
            ShortExits[Now] = Trade.IsShortExit;
        }

        if (!Context.IsOptimization)
            DoDraw();
    }

    public int Now { get; private set; }

    public virtual bool IsLongEnterSignal => false;

    public virtual bool IsLongExitSignal => false;

    public virtual bool IsShortEnterSignal => false;

    public virtual bool IsShortExitSignal => false;

    public virtual double LongEnterVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    public virtual double LongExitVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    public virtual double ShortEnterVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    public virtual double ShortExitVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    protected bool InLongPosition => Trade.InLongPosition;

    protected bool NotInLongPosition => !InLongPosition;

    protected bool InShortPosition => Trade.InShortPosition;

    protected bool NotInShortPosition => !InShortPosition;

    protected IPositionInfo LongPosition => Trade.LongPositionInfo;

    protected IPositionInfo ShortPosition => Trade.ShortPositionInfo;

    public virtual double? LongTake => null;

    public virtual double? LongStop => null;

    public virtual double? ShortTake => null;

    public virtual double? ShortStop => null;

    protected int TradeFromBar { get; set; }

    private AntiGap? _antiGap;

    private IAntiGap AntiGap => _antiGap ??= new AntiGap
    {
        Context = Context,
        Security = BasicTimeframe,
        NowProvider = this
    };

    private bool IsLastCandleOfSession => AntiGap.IsLastCandleOfSession;

    private bool IsJustBeforeLastCandleOfSession => AntiGap.IsJustBeforeLastCandleOfSession;

    private bool[]? _longEnterSignals; //TODO убрать рисовалово в отдельный класс

    private bool[]? _longExitSignals;

    private bool[]? _shortEnterSignals;

    private bool[]? _shortExitSignals;

    private bool[] LongEnters => _longEnterSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private bool[] LongExits => _longExitSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private bool[] ShortEnters => _shortEnterSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private bool[] ShortExits => _shortExitSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

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
        //TODO убрать рисовалово в отдельный класс
        Paint.Signal(Select(() => IsLastCandleOfSession || IsJustBeforeLastCandleOfSession), "Антигэп");

        if (IsBasicTimeframeDraw)
            Paint.Candles(BasicTimeframe, BasicTimeframe.Symbol).DecimalPlaces(BasicTimeframe.Decimals);

        if (IsLongTrade)
        {
            Paint.Signal(LongEnters, "Сигнал на вход в лонг", AnimalColor.Bull);
            Paint.Signal(LongExits, "Сигнал на выход из лонга", AnimalColor.Bear);
        }

        if (IsShortTrade)
        {
            Paint.Signal(ShortEnters, "Сигнал на вход в шорт", AnimalColor.Bull);
            Paint.Signal(ShortExits, "Сигнал на выход из шорта", AnimalColor.Bear);
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

    private IIndicators? _indicatorFactory;

    protected IIndicators Indicators => _indicatorFactory ??= new Indicators(Context);

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
            NowProvider = this,
            Signals = this,
            AntiGap = AntiGap
        }
        : new HistoryTrade
        {
            BasicTimeframe = BasicTimeframe,
            TradeRules = this,
            NowProvider = this,
            Signals = this,
            AntiGap = AntiGap
        };

    [Description("Открывать ли длинные позиции")]
    [HandlerParameter(true, "true")]
    public bool IsLongTrade { get; set; }

    [Description("Открывать ли короткие позиции")]
    [HandlerParameter(true, "true")]
    public bool IsShortTrade { get; set; }
}