using MegaTrade.Basic.Drawing;
using MegaTrade.Basic.Gapping;
using MegaTrade.Basic.Trading;
using MegaTrade.Common;
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

        for (var i = TradeFromBar; i < Context.BarsCount; i++)
        {
            Now = i;
            Trade.Do();
            BasicDraw.PushSignals();
        }

        DoDraw();
    }

    private void DoSetup()
    {
        var setup = Setup();
        BasicTimeframe = setup.BasicTimeframe;
        TradeFromBar = setup.MinBarNumberLimits.Concat([Context.TradeFromBar]).Aggregate(Math.Max);
    }

    protected abstract Setup Setup();

    public int Now { get; private set; }

    public virtual bool IsLongEnter => false;

    public virtual bool IsLongExit => false;

    public virtual bool IsShortEnter => false;

    public virtual bool IsShortExit => false;

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

    private IBasicDraw? _basicDraw;

    private IBasicDraw BasicDraw => _basicDraw ??= Context.IsOptimization
        ? new NullBasicDraw()
        : new BasicDraw
        {
            Context = Context,
            BasicTimeframe = BasicTimeframe,
            AntiGap = AntiGap,
            TradeRules = this,
            Selector = this,
            NowProvider = this,
            TradeSignals = Trade
        };

    protected IPaint Paint => BasicDraw.Paint;

    protected IPaint AddPaint(string name) => BasicDraw.AddPaint(name);

    private void DoDraw()
    {
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

    public IContext Context
    {
        get => Local.Context!;
        set => Local.Context = value;
    }

    private ISecurity BasicTimeframe { get; set; } = null!;

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
    [HandlerParameter(true, "true", NotOptimized = true)]
    public bool IsLongTrade { get; set; }

    [Description("Открывать ли короткие позиции")]
    [HandlerParameter(true, "true", NotOptimized = true)]
    public bool IsShortTrade { get; set; }
}