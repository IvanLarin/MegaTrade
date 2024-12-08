using MegaTrade.Common.Extensions;
using MegaTrade.Systems.Basic;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

/*
 * Входить в позицию когда N свечей подряд растущие в направлении их роста.
 * Выход, когда образовалась свеча впротив входа.
 * Установить стоп-лосс на -2% от цены входа.
 * Установить плавающий стоп на +2% от цены входа.
 */

namespace MegaTrade.Systems;

[HandlerName("N однонаправленных свечей")]
[InputsCount(2)]
[Input(0, TemplateTypes.SECURITY, Name = "Источник")]
[Input(1, TemplateTypes.SECURITY, Name = "Таймфрейм")]
[OutputsCount(0)]
public class CandleSeries : SystemBase
{
    protected override bool IsLongEnterSignal =>
        NotInLongPosition &&
        Enumerable.Range(0, CandlesCount)
            .Select((_, i) => IsBullCandle(Now.To(_timeframe) - i))
            .Aggregate((a, b) => a && b);

    protected override bool IsLongExitSignal => InLongPosition && IsBearCandle(Now.To(_timeframe));

    private bool IsBullCandle(int barIndex) =>
        _timeframe.Bars[barIndex].Close > _timeframe.Bars[barIndex].Open;

    private bool IsBearCandle(int barIndex) =>
        _timeframe.Bars[barIndex].Close < _timeframe.Bars[barIndex].Open;

    public void Execute(ISecurity security, ISecurity timeframe)
    {
        Setup(security);
        TradeFromBar = (CandlesCount + AntiFuture) * timeframe.Interval;

        _timeframe = timeframe;

        Run();
    }

    protected override void Draw() => Paint.Candles(BasicTimeframe).Candles(_timeframe);

    private const int AntiFuture = 1;

    private ISecurity _timeframe = null!;

    [HelperDescription("Число свечей в серии в одном направлении")]
    [HandlerParameter(Name = "Число свечей", IsShown = true, Default = "3", Min = "1", Step = "1")]
    public int CandlesCount { get; set; }
}