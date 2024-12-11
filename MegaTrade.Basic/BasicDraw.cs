using MegaTrade.Basic.Gapping;
using MegaTrade.Common.Extensions;
using MegaTrade.Draw;
using MegaTrade.Draw.Palettes;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Basic;

internal class BasicDraw
{
    public void Draw()
    {
        Paint.Trades(BasicTimeframe);

        Paint.Signal(Selector.Select(() => AntiGap.IsLastCandleOfSession || AntiGap.IsJustBeforeLastCandleOfSession),
            "Антигэп", ScriptColors.LightSlateGray);

        Paint.Signal(Selector.Select(() => true).Select(x => !x).ToArray(), "Меньше TradeFromBar",
            ScriptColors.DarkGray);

        if (TradeRules.IsLongTrade)
        {
            Paint.Signal(LongEnters, "Сигнал на вход в лонг", AnimalColor.Bull);
            Paint.Signal(LongExits, "Сигнал на выход из лонга", AnimalColor.Bear);
        }

        if (TradeRules.IsShortTrade)
        {
            Paint.Signal(ShortEnters, "Сигнал на вход в шорт", AnimalColor.Bull);
            Paint.Signal(ShortExits, "Сигнал на выход из шорта", AnimalColor.Bear);
        }
    }

    public void DrawBasicTimeframe() =>
        Paint.Candles(BasicTimeframe, BasicTimeframe.Symbol).DecimalPlaces(BasicTimeframe.Decimals);


    public IPaint AddPaint(string name) => new Paint
    {
        Context = Context,
        GraphName = name,
        NeutralPalette = _neutralPalette,
        BullPalette = new BullPalette(),
        BearPalette = new BearPalette()
    }.DecimalPlaces(2);

    private bool[]? _longEnterSignals;

    private bool[]? _longExitSignals;

    private bool[]? _shortEnterSignals;

    private bool[]? _shortExitSignals;

    public bool[] LongEnters => _longEnterSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    public bool[] LongExits => _longExitSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    public bool[] ShortEnters => _shortEnterSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    public bool[] ShortExits => _shortExitSignals ??= Context.GetOrCreateArray<bool>(Context.BarsCount);

    private IPaint? _paint;

    public IPaint Paint => _paint ??= AddPaint(BasicTimeframe.Symbol).DecimalPlaces(BasicTimeframe.Decimals);

    private readonly IPalette _neutralPalette = new NeutralPalette();

    public required IContext Context { private get; init; }

    public required ISecurity BasicTimeframe { private get; init; }

    public required ISelector Selector { private get; init; }

    public required IAntiGap AntiGap { private get; init; }

    public required ITradeRules TradeRules { private get; init; }
}