using MegaTrade.Draw.Painters;
using MegaTrade.Draw.Palettes;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Draw;

public class Paint : IPaint
{
    public IPaint Candles(ISecurity security, string? name = null)
    {
        PaintCandles.Candles(security, name);
        return this;
    }

    public IPaint Trades(ISecurity security)
    {
        PaintCandles.Trades(security);
        return this;
    }

    public IPaint Function(IList<double> values, string name)
    {
        PaintFunctions.Function(values, name);
        return this;
    }

    public IPaint Function(IList<double> values, string name, out Color usedColor)
    {
        PaintFunctions.Function(values, name, out usedColor);
        return this;
    }

    public IPaint Function(IList<double> values, string name, AnimalColor animalColor)

    {
        PaintFunctions.Function(values, name, animalColor);
        return this;
    }

    public IPaint Function(IList<double> values, string name, AnimalColor animalColor, out Color usedColor)

    {
        PaintFunctions.Function(values, name, animalColor, out usedColor);
        return this;
    }

    public IPaint Function(IList<double> values, string name, Color color)

    {
        PaintFunctions.Function(values, name, color);
        return this;
    }

    public IPaint Function(IList<double> values, string name, Color color, out Color usedColor)

    {
        PaintFunctions.Function(values, name, color, out usedColor);
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name)
    {
        PaintFunctions.FunctionWithoutZeroes(values, name);
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, out Color usedColor)
    {
        PaintFunctions.FunctionWithoutZeroes(values, name, out usedColor);
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor)

    {
        PaintFunctions.FunctionWithoutZeroes(values, name, animalColor);
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor, out Color usedColor)

    {
        PaintFunctions.FunctionWithoutZeroes(values, name, animalColor, out usedColor);
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, Color color)

    {
        PaintFunctions.FunctionWithoutZeroes(values, name, color);
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, Color color, out Color usedColor)

    {
        PaintFunctions.FunctionWithoutZeroes(values, name, color, out usedColor);
        return this;
    }

    public IPaint Histogram(IList<double> values, string name)
    {
        PaintHistograms.Histogram(values, name);
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, out Color usedColor)

    {
        PaintHistograms.Histogram(values, name, out usedColor);
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, AnimalColor animalColor)
    {
        PaintHistograms.Histogram(values, name, animalColor);
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, AnimalColor animalColor, out Color usedColor)
    {
        PaintHistograms.Histogram(values, name, animalColor, out usedColor);
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, Color color)
    {
        PaintHistograms.Histogram(values, name);
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, Color color, out Color usedColor)
    {
        PaintHistograms.Histogram(values, name, out usedColor);
        return this;
    }

    public IPaint Signal(IList<bool> values, string name)
    {
        PaintSignals.Signal(values, name);
        return this;
    }

    public IPaint Signal(IList<bool> values, string name, out Color usedColor)
    {
        PaintSignals.Signal(values, name, out usedColor);
        return this;
    }

    public IPaint Signal(IList<bool> values, string name, AnimalColor animalColor)
    {
        PaintSignals.Signal(values, name, animalColor);
        return this;
    }

    public IPaint Signal(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor)
    {
        PaintSignals.Signal(values, name, animalColor, out usedColor);
        return this;
    }

    public IPaint Signal(IList<bool> values, string name, Color color)
    {
        PaintSignals.Signal(values, name, color);
        return this;
    }

    public IPaint Signal(IList<bool> values, string name, Color color, out Color usedColor)
    {
        PaintSignals.Signal(values, name, color, out usedColor);
        return this;
    }

    public IPaint Level(double value, string name, Color color)
    {
        PaintLevels.Level(value, name, color);
        return this;
    }

    public IPaint Level(double value, string name, Color color, out Color usedColor)
    {
        PaintLevels.Level(value, name, color, out usedColor);
        return this;
    }

    public IPaint Bound(params double[] bounds)
    {
        PaintBounds.Bound(bounds);
        return this;
    }

    public IPaint LeftDecimalPlaces(int count)
    {
        Graph.UpdatePrecision(PaneSides.LEFT, count);
        return this;
    }

    public IPaint RightDecimalPlaces(int count)
    {
        Graph.UpdatePrecision(PaneSides.RIGHT, count);
        return this;
    }

    public IPaint Level(double value, string name, AnimalColor animalColor)
    {
        PaintLevels.Level(value, name, animalColor);
        return this;
    }

    public IPaint Level(double value, string name, AnimalColor animalColor, out Color usedColor)
    {
        PaintLevels.Level(value, name, animalColor, out usedColor);
        return this;
    }


    private IGraphPane? _graph;

    private IGraphPane Graph => _graph ??= Context.Panes.FirstOrDefault(x => x.Name == GraphName) as IGraphPane ??
                                           Context.CreateGraphPane(GraphName, null, AddToTop);

    private IPaintCandles? _paintCandles;

    private IPaintCandles PaintCandles => _paintCandles ??= new PaintCandles { Graph = Graph };

    private IPaintFunctions? _paintFunctions;

    private IPaintFunctions PaintFunctions =>
        _paintFunctions ??= new PaintFunctions
        {
            Graph = Graph,
            BullPalette = BullPalette,
            BearPalette = BearPalette,
            NeutralPalette = NeutralPalette
        };

    private IPaintSignals? _paintSignals;

    private IPaintSignals PaintSignals =>
        _paintSignals ??= new PaintSignals
        {
            Graph = Graph,
            BullPalette = BullPalette,
            BearPalette = BearPalette,
            NeutralPalette = NeutralPalette
        };

    private IPaintHistograms? _paintHistograms;

    private IPaintHistograms PaintHistograms =>
        _paintHistograms ??= new PaintHistograms
        {
            Graph = Graph,
            BullPalette = BullPalette,
            BearPalette = BearPalette,
            NeutralPalette = NeutralPalette
        };

    private IPaintLevels? _paintLevels;

    private IPaintLevels PaintLevels =>
        _paintLevels ??= new PaintLevels
        {
            Graph = Graph,
            BullPalette = BullPalette,
            BearPalette = BearPalette,
            NeutralPalette = NeutralPalette,
            Context = Context
        };

    private IPaintBounds? _paintBounds;

    private IPaintBounds PaintBounds =>
        _paintBounds ??= new PaintBounds
        {
            Graph = Graph,
            Context = Context
        };

    public required string GraphName { private get; init; }

    public required IContext Context { private get; init; }

    public bool AddToTop { private get; init; } = false;

    public required IPalette BullPalette { private get; init; }

    public required IPalette BearPalette { private get; init; }

    public required IPalette NeutralPalette { private get; init; }
}