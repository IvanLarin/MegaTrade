using MegaTrade.Common.Extensions;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace MegaTrade.Systems;

[HandlerCategory("МегаСистемы")]
[HandlerName("Моментум двух таймфреймов")]
[InputsCount(3)]
[Input(0, TemplateTypes.SECURITY, Name = "Источник")]
[Input(1, TemplateTypes.SECURITY, Name = "Меньший таймфрейм")]
[Input(2, TemplateTypes.SECURITY, Name = "Больший таймфрейм")]
[OutputsCount(0)]
public class TwoMomentums : SystemBase
{
    protected override bool IsLongEnterSignal => NoLongPosition &&
                                                 _smallTimeframeMacd[Now - 1] <= 0 &&
                                                 _smallTimeframeMacd[Now] > 0 &&
                                                 _bigTimeframeMacd[Now] > 0 &&
                                                 _bigTimeframeRsi[Now] < OverboughtRsiOfBig &&
                                                 _smallTimeframeRsi[Now] < OverboughtRsiOfSmall;

    protected override bool IsLongExitSignal => _smallTimeframeMacd[Now] <= 0;

    protected override bool IsShortEnterSignal => NoShortPosition &&
                                                  _smallTimeframeMacd[Now - 1] >= 0 &&
                                                  _smallTimeframeMacd[Now] < 0 &&
                                                  _bigTimeframeMacd[Now] < 0 &&
                                                  _bigTimeframeRsi[Now] > OversoldRsiOfBig &&
                                                  _smallTimeframeRsi[Now] > OversoldRsiOfSmall;

    protected override bool IsShortExitSignal => _smallTimeframeMacd[Now] >= 0;

    public void Execute(ISecurity security, ISecurity smallTimeframe, ISecurity bigTimeframe)
    {
        Setup(security);
        _smallTimeframe = smallTimeframe;
        _bigTimeframe = bigTimeframe;

        _smallTimeframeClosePrices = smallTimeframe.ClosePrices;
        _bigTimeframeClosePrices = bigTimeframe.ClosePrices;

        _smallTimeframeMacd = IndicatorFactory
            .MakeMacd(_smallTimeframeClosePrices, SmallMacdOfSmall, BigMacdOfSmall)
            .DecompressBy(smallTimeframe);

        _bigTimeframeMacd = IndicatorFactory
            .MakeMacd(_bigTimeframeClosePrices, SmallMacdOfBig, BigMacdOfBig)
            .DecompressBy(bigTimeframe);

        _smallTimeframeRsi = IndicatorFactory
            .MakeRsi(_smallTimeframeClosePrices, RsiPeriodOfSmall)
            .DecompressBy(smallTimeframe);

        _bigTimeframeRsi = IndicatorFactory
            .MakeRsi(_bigTimeframeClosePrices, RsiPeriodOfBig)
            .DecompressBy(bigTimeframe);

        Run();
    }

    private ISecurity _smallTimeframe = null!;
    private ISecurity _bigTimeframe = null!;

    private IList<double> _smallTimeframeClosePrices = [];
    private IList<double> _bigTimeframeClosePrices = [];
    private IList<double> _smallTimeframeMacd = [];
    private IList<double> _bigTimeframeMacd = [];
    private IList<double> _smallTimeframeRsi = [];
    private IList<double> _bigTimeframeRsi = [];

    protected override bool IsBasicTimeframeDraw => false;

    protected override void Draw()
    {
        //TODO сигналы должен рисовать базовый класс

        Paint.Candles(_smallTimeframe, "Меньший таймфрейм");
        Paint.Candles(_bigTimeframe, "Больший таймфрейм");
    }


    [HelperDescription("Малый период MACD меньшего таймфрейма")]
    [HandlerParameter(Name = "SmallMacdOfSmall", IsShown = true, Default = "12", Min = "2", Step = "1")]
    public int SmallMacdOfSmall { get; set; }

    [HelperDescription("Больший период MACD меньшего таймфрейма")]
    [HandlerParameter(Name = "BigMacdOfSmall", IsShown = true, Default = "26", Min = "3", Step = "1")]
    public int BigMacdOfSmall { get; set; }

    [HelperDescription("Малый период MACD большего таймфрейма")]
    [HandlerParameter(Name = "SmallMacdOfBig", IsShown = true, Default = "12", Min = "2", Step = "1")]
    public int SmallMacdOfBig { get; set; }

    [HelperDescription("Больший период MACD большего таймфрейма")]
    [HandlerParameter(Name = "BigMacdOfBig", IsShown = true, Default = "26", Min = "3", Step = "1")]
    public int BigMacdOfBig { get; set; }

    [HelperDescription("Период RSI меньшего таймфрейма")]
    [HandlerParameter(Name = "RsiPeriodOfSmall", IsShown = true, Default = "20", Min = "1", Step = "1")]
    public int RsiPeriodOfSmall { get; set; }

    [HelperDescription("Период RSI большего таймфрейма")]
    [HandlerParameter(Name = "RsiPeriodOfBig", IsShown = true, Default = "20", Min = "1", Step = "1")]
    public int RsiPeriodOfBig { get; set; }

    [HelperDescription("Перекупленность RSI большего тайфрейма")]
    [HandlerParameter(Name = "OverboughtRsiOfBig", IsShown = true, Default = "70", Min = "1", Max = "100", Step = "1")]
    public double OverboughtRsiOfBig { get; set; }

    [HelperDescription("Перекупленность RSI меньшего тайфрейма")]
    [HandlerParameter(Name = "OverboughtRsiOfSmall", IsShown = true, Default = "70", Min = "1", Max = "100",
        Step = "1")]
    public double OverboughtRsiOfSmall { get; set; }

    [HelperDescription("Перепроданность RSI меньшего тайфрейма")]
    [HandlerParameter(Name = "OversoldRsiOfSmall", IsShown = true, Default = "30", Min = "1", Max = "100", Step = "1")]
    public double OversoldRsiOfSmall { get; set; }

    [HelperDescription("Перепроданность RSI большего тайфрейма")]
    [HandlerParameter(Name = "OversoldRsiOfBig", IsShown = true, Default = "30", Min = "1", Max = "100", Step = "1")]
    public double OversoldRsiOfBig { get; set; }
}