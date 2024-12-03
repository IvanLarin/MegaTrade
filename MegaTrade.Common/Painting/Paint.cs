using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Painting;

public class Paint : IPaint
{
    public void Candles(ISecurity security, string? name = null) => PaintCandles.Candles(security, name);

    public void Trades(ISecurity security) => PaintCandles.Trades(security);

    public void Function(IList<double> values, string name, Color? color = null) =>
        PaintFunctions.Function(values, name, color);

    public void Histogram(IList<bool> values, string name, AnimalColor animalColor = AnimalColor.Neutral) =>
        PaintHistogram.Histogram(values, name, animalColor);

    public void Histogram(IList<bool> values, string name, Color color) => PaintHistogram.Histogram(values, name);

    public void Signal(IList<bool> values, string name, AnimalColor animalColor = AnimalColor.Neutral) =>
        PaintHistogram.Signal(values, name, animalColor);

    public void Signal(IList<bool> values, string name, Color color) => PaintHistogram.Signal(values, name, color);

    private IGraphPane? _graph;

    private IGraphPane Graph => _graph ??= Context.Panes.FirstOrDefault(x => x.Name == GraphName) as IGraphPane ??
                                           Context.CreateGraphPane(GraphName, null, AddToTop);

    private IPaintCandles? _paintCandles;

    private IPaintCandles PaintCandles => _paintCandles ??= new PaintCandles { Graph = Graph };

    private IPaintFunctions? _paintFunctions;

    private IPaintFunctions PaintFunctions =>
        _paintFunctions ??= new PaintFunctions { Graph = Graph, Palette = NeutralPalette };

    private IPaintHistogram? _paintHistogram;

    private IPaintHistogram PaintHistogram =>
        _paintHistogram ??= new PaintHistogram
        {
            Graph = Graph,
            BullPalette = BullPalette,
            BearPalette = BearPalette,
            NeutralPalette = NeutralPalette
        };

    public required string GraphName { private get; init; }

    public required IContext Context { private get; init; }

    public bool AddToTop { private get; init; } = false;

    public required IPalette BullPalette { private get; init; }

    public required IPalette BearPalette { private get; init; }

    public required IPalette NeutralPalette { private get; init; }
}