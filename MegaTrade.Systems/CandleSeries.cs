using MegaTrade.Basic;
using MegaTrade.Common.Extensions;
using MegaTrade.Indicators;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace MegaTrade.Systems;

[HandlerName("N однонаправленных свечей")]
[InputsCount(2)]
[Input(0, TemplateTypes.SECURITY, Name = "Источник")]
[Input(1, TemplateTypes.SECURITY, Name = "Таймфрейм")]
[OutputsCount(0)]
public class CandleSeries : SystemBase
{
    public override bool IsLongEnter =>
        NotInLongPosition &&
        Enumerable.Range(0, CandlesCount)
            .Select((_, i) => IsBearCandle(Now.To(_timeframe) - i))
            .Aggregate((a, b) => a && b);

    public override bool IsShortEnter =>
        NotInShortPosition &&
        Enumerable.Range(0, CandlesCount)
            .Select((_, i) => IsBullCandle(Now.To(_timeframe) - i))
            .Aggregate((a, b) => a && b);

    private bool IsBullCandle(int barIndex) =>
        _timeframe.Bars[barIndex].Close > _timeframe.Bars[barIndex].Open;

    private bool IsBearCandle(int barIndex) =>
        _timeframe.Bars[barIndex].Close < _timeframe.Bars[barIndex].Open;

    public override double? LongTakeProfit =>
        LongPosition.EntryPrice + AtrMultiplier * _atr[LongPosition.EntryBarNum.To(_timeframe)];

    public override double? LongStopLoss =>
        LongPosition.EntryPrice - AtrMultiplier * _atr[LongPosition.EntryBarNum.To(_timeframe)];

    public override double? ShortTakeProfit =>
        ShortPosition.EntryPrice - AtrMultiplier * _atr[ShortPosition.EntryBarNum.To(_timeframe)];

    public override double? ShortStopLoss =>
        ShortPosition.EntryPrice + AtrMultiplier * _atr[ShortPosition.EntryBarNum.To(_timeframe)];

    public void Execute(ISecurity basicTimeframe, ISecurity timeframe)
    {
        _basicTimeframe = basicTimeframe;
        _timeframe = timeframe;
        _atr = timeframe.ATR(AtrPeriod);

        Run();
    }

    protected override Setup Setup() => new()
    {
        BasicTimeframe = _basicTimeframe,
        MinBarNumberLimits = [CandlesCount * _timeframe.Interval, AtrPeriod]
    };

    protected override void Draw() => Paint.Candles(_basicTimeframe).Candles(_timeframe);

    private ISecurity _basicTimeframe = null!;
    private ISecurity _timeframe = null!;
    private IList<double> _atr = [];

    [HelperDescription("Число свечей в серии в одном направлении")]
    [HandlerParameter(Name = "Число свечей", Default = "3", Min = "1", Step = "1")]
    public int CandlesCount { get; set; }

    [HandlerParameter(Default = "20", Min = "1", Step = "1")]
    public int AtrPeriod { get; set; }

    [HandlerParameter(Default = "2", Min = "0.1", Step = "0.1")]
    public double AtrMultiplier { get; set; }
}