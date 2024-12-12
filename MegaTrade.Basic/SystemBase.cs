using MegaTrade.Basic.Gapping;
using MegaTrade.Basic.Indicating;
using MegaTrade.Basic.Trading;
using MegaTrade.Common.Caching;
using MegaTrade.Common.Extensions;
using MegaTrade.Draw;
using System.ComponentModel;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Basic;

[HandlerCategory("МегаСистемы")]
public abstract class SystemBase : IHandler, IContextUses, ITradeRules, INowProvider, ISignals, ISelector
{
    protected void Run()
    {
        DoSetup();

        for (var i = Math.Max(TradeFromBar, Context.TradeFromBar); i < Context.BarsCount; i++)
        {
            Now = i;

            Trade.Do();

            BasicDraw.LongEnters[Now] = Trade.IsLongEnter;
            BasicDraw.LongExits[Now] = Trade.IsLongExit;
            BasicDraw.ShortEnters[Now] = Trade.IsShortEnter;
            BasicDraw.ShortExits[Now] = Trade.IsShortExit;
        }

        if (!Context.IsOptimization)
            DoDraw();
    }

    private void DoSetup()
    {
        var setup = Setup();
        BasicTimeframe = setup.BasicTimeframe;
        TradeFromBar = setup.MinBarNumberLimits.Concat([Context.TradeFromBar]).Aggregate(Math.Max);

        Cache.Context = Context;
    }

    protected abstract Setup Setup();

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

    private int TradeFromBar { get; set; }

    private IAntiGap? _antiGap;

    private IAntiGap AntiGap => _antiGap ??= new AntiGap
    {
        Context = Context,
        Security = BasicTimeframe,
        NowProvider = this
    };

    private BasicDraw? _basicDraw;

    private BasicDraw BasicDraw => _basicDraw ??= new BasicDraw
    {
        Context = Context,
        BasicTimeframe = BasicTimeframe,
        AntiGap = AntiGap,
        TradeRules = this,
        Selector = this
    };

    protected IPaint Paint => BasicDraw.Paint;

    protected IPaint AddPaint(string name) => BasicDraw.AddPaint(name);

    protected virtual bool IsBasicTimeframeDraw => true;

    private void DoDraw()
    {
        if (IsBasicTimeframeDraw)
            BasicDraw.DrawBasicTimeframe();

        BasicDraw.Draw();

        Draw();
    }

    public IList<T> Select<T>(Func<T> func) where T : struct
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

    protected IIndicators Indicators => _indicatorFactory ??= new Indicators();

    private IContext? _context;

    public IContext Context
    {
        get => _context!;
        set => _context = value;
    }

    protected ISecurity BasicTimeframe
    {
        get;
        private set;
    } = null!;

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