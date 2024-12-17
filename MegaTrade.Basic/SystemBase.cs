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

/// <summary>
/// Базовый класс для торговой системы.
/// <para>Наследник <see cref="SystemBase"/> в методе <c>public Execute</c> должен инициализировать свои переменные и вызвать метод <see cref="Run"/>.</para>
/// </summary>
[HandlerCategory("МегаСистемы")]
public abstract class SystemBase : IHandler, IContextUses, ITradeRules, INowProvider, ISignals, ISelector
{
    /// <summary>
    /// Запускает выполнение торговой системы.
    /// </summary>
    protected void Run()
    {
        DoSetup();

        for (Now = TradeFromBar; Now < Context.BarsCount; Now++)
        {
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

    /// <summary>
    /// Получает настройки для торговой системы
    /// </summary>
    protected abstract Setup Setup();

    /// <summary>
    /// Указывает текущий индекс бара для которого рассчитываются свойства данного класса.
    /// </summary>
    public int Now { get; private set; }

    /// <summary>
    /// Задаёт, следует ли открывать длинную позицию на текущем индексе бара.
    /// </summary>
    public virtual bool IsLongEnter => false;

    /// <summary>
    /// Задаёт, следует ли закрывать длинную позицию на текущем индексе бара.
    /// </summary>
    public virtual bool IsLongExit => false;

    /// <summary>
    /// Задаёт, следует ли открывать короткую позицию на текущем индексе бара.
    /// </summary>
    public virtual bool IsShortEnter => false;

    /// <summary>
    /// Задаёт, следует ли закрывать короткую позицию на текущем индексе бара.
    /// </summary>
    public virtual bool IsShortExit => false;

    /// <summary>
    /// Задаёт объем для открытия длинной позиции на текущем индексе бара.
    /// </summary>
    public virtual double LongEnterVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    /// <summary>
    /// Задаёт объем для закрытия длинной позиции на текущем индексе бара.
    /// </summary>
    public virtual double LongExitVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    /// <summary>
    /// Задаёт объем для открытия короткой позиции на текущем индексе бара.
    /// </summary>
    public virtual double ShortEnterVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    /// <summary>
    /// Задаёт объем для закрытия короткой позиции на текущем индексе бара.
    /// </summary>
    public virtual double ShortExitVolume =>
        BasicTimeframe.LotSize; //TODO каким количеством торговать, если счёт слился?

    /// <summary>
    /// Указывает открыта ли длинная позиция на текущем индексе бара.
    /// </summary>
    protected bool InLongPosition => Trade.InLongPosition;

    /// <summary>
    /// Указывает не открыта ли длинная позиция на текущем индексе бара.
    /// </summary>
    protected bool NotInLongPosition => !InLongPosition;

    /// <summary>
    /// Указывает открыта ли короткая позиция на текущем индексе бара.
    /// </summary>
    protected bool InShortPosition => Trade.InShortPosition;

    /// <summary>
    /// Указывает не открыта ли короткая позиция на текущем индексе бара.
    /// </summary>
    protected bool NotInShortPosition => !InShortPosition;

    /// <summary>
    /// Предоставляет данные по текущей длинной позиции.
    /// </summary>
    protected IPositionInfo LongPosition => Trade.LongPositionInfo;

    /// <summary>
    /// Предоставляет данные по текущей короткой позиции.
    /// </summary>
    protected IPositionInfo ShortPosition => Trade.ShortPositionInfo;

    /// <summary>
    /// Задаёт значение для тейк-профита длинной позиции <see cref="Now"/>.
    /// Если значение равно null, это означает, что тейк-профит для длинной позиции не должен выставляться.
    /// </summary>
    public virtual double? LongTakeProfit => null;

    /// <summary>
    /// Задаёт значение для стоп-лосса длинной позиции <see cref="Now"/>.
    /// Если значение равно null, это означает, что стоп-лосс для длинной позиции не должен выставляться.
    /// </summary>
    public virtual double? LongStopLoss => null;

    /// <summary>
    /// Задаёт значение для тейк-профита короткой позиции <see cref="Now"/>.
    /// Если значение равно null, это означает, что тейк-профит для короткой позиции не должен выставляться.
    /// </summary>
    public virtual double? ShortTakeProfit => null;

    /// <summary>
    /// Задаёт значение для стоп-лосса короткой позиции <see cref="Now"/>.
    /// Если значение равно null, это означает, что стоп-лосс для короткой позиции не должен выставляться.
    /// </summary>
    public virtual double? ShortStopLoss => null;

    /// <summary>
    /// Выбирает данные на основе заданной функции, которая зависит от <see cref="Now"/>.
    /// </summary>
    public IList<T> Select<T>(Func<T> func) where T : struct
    {
        T[] result = Context.GetOrCreateArray<T>(Context.BarsCount);

        for (Now = TradeFromBar; Now < Context.BarsCount; Now++)
            result[Now] = func();

        return result;
    }

    /// <summary>
    /// Предоставляет <see cref="IPaint"/> для рисования на главной панели графика.
    /// </summary>
    protected IPaint Paint => BasicDraw.Paint;


    /// <summary>
    /// Выполняет отрисовку.
    /// </summary>
    protected virtual void Draw()
    {
    }

    private void DoDraw()
    {
        BasicDraw.Draw();
        Draw();
    }

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