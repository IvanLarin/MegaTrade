using MegaTrade.Basic;
using MegaTrade.Common.Extensions;
using MegaTrade.Draw;
using MegaTrade.Indicators;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace MegaTrade.Systems;

[HandlerName("НадараяВатсон")]
[InputsCount(2)]
[Input(0, TemplateTypes.SECURITY, Name = "Источник")]
[Input(1, TemplateTypes.SECURITY, Name = "Таймфрейм")]
[OutputsCount(0)]
public class NadarayaWatson : SystemBase
{
    public override bool IsLongEnter =>
        NotInLongPosition && _closePrices[Now - 1] <= _nadarayaDown[Now - 1] && _closePrices[Now] > _nadarayaDown[Now];

    public override bool IsLongExit =>
        _closePrices[Now - 1] >= _nadarayaUp[Now - 1] && _closePrices[Now] < _nadarayaUp[Now];

    public override bool IsShortEnter => NotInShortPosition && _closePrices[Now - 1] >= _nadarayaUp[Now - 1] && _closePrices[Now] < _nadarayaUp[Now];

    public override bool IsShortExit => _closePrices[Now - 1] <= _nadarayaDown[Now - 1] && _closePrices[Now] > _nadarayaDown[Now];

    public void Execute(ISecurity basicTimeframe, ISecurity timeframe)
    {
        _basicTimeframe = basicTimeframe;
        _timeframe = timeframe;
        _closePrices = basicTimeframe.ClosePrices;
        _nadarayaUp = timeframe.ClosePrices.NadarayaWatsonUpper(Bandwidth, Multiplier, Range)
            .DecompressFrom(timeframe);
        _nadarayaDown = timeframe.ClosePrices.NadarayaWatsonLower(Bandwidth, Multiplier, Range)
            .DecompressFrom(timeframe);

        Run();
    }

    protected override Setup Setup() => new()
    {
        BasicTimeframe = _basicTimeframe,
        MinBarNumberLimits = [Range]
    };

    protected override void Draw() => Paint
        .Candles(_basicTimeframe)
        .Candles(_timeframe)
        .FunctionWithoutZeroes(_nadarayaUp, "Надарая-Ватсон", AnimalColor.Bull)
        .FunctionWithoutZeroes(_nadarayaDown, "Надарая-Ватсон", AnimalColor.Bear);

    private ISecurity _basicTimeframe = null!;
    private ISecurity _timeframe = null!;
    private IList<double> _nadarayaUp = [];
    private IList<double> _nadarayaDown = [];

    private IList<double> _closePrices = [];

    [HelperDescription("Range")]
    [HandlerParameter(Default = "500", Min = "2", Step = "1")]
    public int Range { get; set; }

    [HelperDescription("Multiplier")]
    [HandlerParameter(Default = "3", Min = "0.1", Step = "0.1")]
    public double Multiplier { get; set; }

    [HelperDescription("Bandwidth")]
    [HandlerParameter(Default = "8", Min = "0.1", Step = "0.1")]
    public double Bandwidth { get; set; }
}