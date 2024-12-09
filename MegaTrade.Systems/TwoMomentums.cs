﻿using MegaTrade.Basic;
using MegaTrade.Common.Extensions;
using MegaTrade.Draw;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Handlers.Options;

namespace MegaTrade.Systems;

[HandlerName("Моментум двух таймфреймов")]
[InputsCount(3)]
[Input(0, TemplateTypes.SECURITY, Name = "Источник")]
[Input(1, TemplateTypes.SECURITY, Name = "Меньший таймфрейм")]
[Input(2, TemplateTypes.SECURITY, Name = "Больший таймфрейм")]
[OutputsCount(0)]
public class TwoMomentums : SystemBase
{
    public override bool IsLongEnterSignal =>
        IsBullMacdStart &&
        !IsRsiOverbought &&
        NotInLongPosition;

    public override bool IsLongExitSignal =>
        _smallTimeframeMacd[Now - 1] > 0 &&
        _smallTimeframeMacd[Now] <= 0;

    public override bool IsShortEnterSignal =>
        IsBearMacdStart &&
        !IsRsiOversold &&
        NotInShortPosition;

    public override bool IsShortExitSignal =>
        _smallTimeframeMacd[Now - 1] < 0 &&
        _smallTimeframeMacd[Now] >= 0;

    private bool IsBullMacdStart =>
        _smallTimeframeMacd[Now - 1] <= 0 &&
        _smallTimeframeMacd[Now] > 0 &&
        _bigTimeframeMacd[Now] > 0;

    private bool IsRsiOverbought => IsBigRsiOverbought ||
                                    IsSmallRsiOverbought;

    private bool IsBigRsiOverbought => _bigTimeframeRsi[Now] >= OverboughtRsiOfBig;

    private bool IsSmallRsiOverbought => _smallTimeframeRsi[Now] >= OverboughtRsiOfSmall;

    private bool IsBearMacdStart =>
        _smallTimeframeMacd[Now - 1] >= 0 &&
        _smallTimeframeMacd[Now] < 0 &&
        _bigTimeframeMacd[Now] < 0;

    private bool IsRsiOversold => IsBigRsiOversold ||
                                  IsSmallRsiOversold;

    private bool IsBigRsiOversold => _bigTimeframeRsi[Now] <= OversoldRsiOfBig;

    private bool IsSmallRsiOversold => _smallTimeframeRsi[Now] <= OversoldRsiOfSmall;

    public void Execute(ISecurity security, ISecurity smallTimeframe, ISecurity bigTimeframe)
    {
        Setup(security);
        _smallTimeframe = smallTimeframe;
        _bigTimeframe = bigTimeframe;

        _smallTimeframeClosePrices = smallTimeframe.ClosePrices;
        _bigTimeframeClosePrices = bigTimeframe.ClosePrices;

        _smallTimeframeMacd = Indicators
            .MACD(_smallTimeframeClosePrices, SmallMacdOfSmall, BigMacdOfSmall)
            .DecompressBy(smallTimeframe);

        _bigTimeframeMacd = Indicators
            .MACD(_bigTimeframeClosePrices, SmallMacdOfBig, BigMacdOfBig)
            .DecompressBy(bigTimeframe);

        _smallTimeframeRsi = Indicators
            .RSI(_smallTimeframeClosePrices, RsiPeriodOfSmall)
            .DecompressBy(smallTimeframe);

        _bigTimeframeRsi = Indicators
            .RSI(_bigTimeframeClosePrices, RsiPeriodOfBig)
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
        Paint.Candles(_smallTimeframe);
        Paint.Candles(_bigTimeframe);

        DrawMacd();
        DrawRsi();
    }

    private void DrawMacd() => AddPaint("MACD")
        .Level(0, "Ноль", ScriptColors.White)
        .Function(_smallTimeframeMacd, "MACD меньшего таймфрейма")
        .Function(_bigTimeframeMacd, "MACD большего таймфрейма")
        .Signal(Select(() => IsBullMacdStart),
            "Бычий разворот меньшего таймрейма в направлении большего таймрейма", AnimalColor.Bull)
        .Signal(Select(() => IsLongExitSignal), "Медвежий разворот меньшего таймфрейма", AnimalColor.Bear);

    private void DrawRsi() => AddPaint("RSI")
        .Bound(0, 100)
        .Function(_smallTimeframeRsi, "RSI меньшего таймфрейма", out var smallRsiColor)
        .Level(OverboughtRsiOfSmall, "Уровень перекупленности меньшего таймфрейма", smallRsiColor)
        .Level(OversoldRsiOfSmall, "Уровень перепроданности меньшего таймфрейма", smallRsiColor)
        .Signal(Select(() => IsSmallRsiOverbought), "Перекупленность меньшего таймфрейма", smallRsiColor)
        .Signal(Select(() => IsSmallRsiOversold), "Перепроданность меньшего таймфрейма", smallRsiColor)
        .Function(_bigTimeframeRsi, "RSI большего таймфрейма", out var bigRsiColor)
        .Level(OverboughtRsiOfBig, "Уровень перекупленности большего таймфрейма", bigRsiColor)
        .Level(OversoldRsiOfBig, "Уровень перепроданности большего таймфрейма", bigRsiColor)
        .Signal(Select(() => IsBigRsiOverbought), "Перекупленность большего таймфрейма", bigRsiColor)
        .Signal(Select(() => IsBigRsiOversold), "Перепроданность большего таймфрейма", bigRsiColor)
        .Signal(Select(() => IsRsiOverbought), "Перекупленность", AnimalColor.Bear)
        .Signal(Select(() => IsRsiOversold), "Перепроданность", AnimalColor.Bear);

    [HelperDescription("Малый период MACD меньшего таймфрейма")]
    [HandlerParameter(Name = "SmallMacdOfSmall", Default = "12", Min = "2", Step = "1")]
    public int SmallMacdOfSmall { get; set; }

    [HelperDescription("Больший период MACD меньшего таймфрейма")]
    [HandlerParameter(Name = "BigMacdOfSmall", Default = "26", Min = "3", Step = "1")]
    public int BigMacdOfSmall { get; set; }

    [HelperDescription("Малый период MACD большего таймфрейма")]
    [HandlerParameter(Name = "SmallMacdOfBig", Default = "12", Min = "2", Step = "1")]
    public int SmallMacdOfBig { get; set; }

    [HelperDescription("Больший период MACD большего таймфрейма")]
    [HandlerParameter(Name = "BigMacdOfBig", Default = "26", Min = "3", Step = "1")]
    public int BigMacdOfBig { get; set; }

    [HelperDescription("Период RSI меньшего таймфрейма")]
    [HandlerParameter(Name = "RsiPeriodOfSmall", Default = "20", Min = "1", Step = "1")]
    public int RsiPeriodOfSmall { get; set; }

    [HelperDescription("Период RSI большего таймфрейма")]
    [HandlerParameter(Name = "RsiPeriodOfBig", Default = "20", Min = "1", Step = "1")]
    public int RsiPeriodOfBig { get; set; }

    [HelperDescription("Перекупленность RSI большего тайфрейма")]
    [HandlerParameter(Name = "OverboughtRsiOfBig", Default = "70", Min = "1", Max = "100", Step = "1")]
    public double OverboughtRsiOfBig { get; set; }

    [HelperDescription("Перекупленность RSI меньшего тайфрейма")]
    [HandlerParameter(Name = "OverboughtRsiOfSmall", Default = "70", Min = "1", Max = "100",
        Step = "1")]
    public double OverboughtRsiOfSmall { get; set; }

    [HelperDescription("Перепроданность RSI меньшего тайфрейма")]
    [HandlerParameter(Name = "OversoldRsiOfSmall", Default = "30", Min = "1", Max = "100", Step = "1")]
    public double OversoldRsiOfSmall { get; set; }

    [HelperDescription("Перепроданность RSI большего тайфрейма")]
    [HandlerParameter(Name = "OversoldRsiOfBig", Default = "30", Min = "1", Max = "100", Step = "1")]
    public double OversoldRsiOfBig { get; set; }
}