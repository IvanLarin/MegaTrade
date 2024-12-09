using MegaTrade.Basic;
using MegaTrade.Common.Extensions;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace MegaTrade.Systems;

[HandlerName("N ���������������� ������")]
[InputsCount(2)]
[Input(0, TemplateTypes.SECURITY, Name = "��������")]
[Input(1, TemplateTypes.SECURITY, Name = "���������")]
[OutputsCount(0)]
public class CandleSeries : SystemBase
{
    protected override bool IsLongEnterSignal =>
        NotInLongPosition &&
        Enumerable.Range(0, CandlesCount)
            .Select((_, i) => IsBearCandle(Now.To(_timeframe) - i))
            .Aggregate((a, b) => a && b);

    protected override bool IsShortEnterSignal =>
        NotInShortPosition &&
        Enumerable.Range(0, CandlesCount)
            .Select((_, i) => IsBullCandle(Now.To(_timeframe) - i))
            .Aggregate((a, b) => a && b);

    private bool IsBullCandle(int barIndex) =>
        _timeframe.Bars[barIndex].Close > _timeframe.Bars[barIndex].Open;

    private bool IsBearCandle(int barIndex) =>
        _timeframe.Bars[barIndex].Close < _timeframe.Bars[barIndex].Open;

    public override double? LongTake =>
        LongPosition!.EntryPrice + AtrMultiplier * _atr[LongPosition.EntryBarNum.To(_timeframe)];

    public override double? LongStop =>
        LongPosition!.EntryPrice - AtrMultiplier * _atr[LongPosition.EntryBarNum.To(_timeframe)];

    public override double? ShortTake =>
        ShortPosition!.EntryPrice - AtrMultiplier * _atr[ShortPosition.EntryBarNum.To(_timeframe)];

    public override double? ShortStop =>
        ShortPosition!.EntryPrice + AtrMultiplier * _atr[ShortPosition.EntryBarNum.To(_timeframe)];

    public void Execute(ISecurity security, ISecurity timeframe)
    {
        Setup(security);
        TradeFromBar = Math.Max((CandlesCount + AntiFuture) * timeframe.Interval, AtrPeriod); //TODO ���� �� ������������� ���, � ������ ������������ �����

        _timeframe = timeframe;

        _atr = Indicators.ATR(timeframe, AtrPeriod);

        Run();
    }

    protected override void Draw() => Paint.Candles(BasicTimeframe).Candles(_timeframe);

    private const int AntiFuture = 1;

    private ISecurity _timeframe = null!;

    private IList<double> _atr = [];

    [HelperDescription("����� ������ � ����� � ����� �����������")]
    [HandlerParameter(Name = "����� ������", Default = "3", Min = "1", Step = "1")]
    public int CandlesCount { get; set; }

    [HandlerParameter(Default = "20", Min = "1", Step = "1")]
    public int AtrPeriod { get; set; }

    [HandlerParameter(Default = "2", Min = "0.1", Step = "0.1")]
    public double AtrMultiplier { get; set; }
}