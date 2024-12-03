using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Painting;

public class Paint : IPaint
{
    private readonly IPaintCandles _paintCandles;

    private readonly IPaintFunctions _paintLines;

    public Paint(IContext context, string graphName, IPalette palette, bool addToTop = false)
    {
        var graph = context.Panes.FirstOrDefault(x => x.Name == graphName) as IGraphPane ??
                    context.CreateGraphPane(graphName, null, addToTop);

        _paintCandles = new PaintCandles(graph);

        _paintLines = new PaintFunctions(graph, palette);
    }

    public void Candles(ISecurity security, string name) => _paintCandles.Candles(security, name);

    public void Trades(ISecurity security) => _paintCandles.Trades(security);

    public void Function(IList<double> values, string name, Color? color = null) => _paintLines.Function(values, name, color);
}