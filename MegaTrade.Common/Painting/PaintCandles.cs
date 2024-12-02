using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal class PaintCandles : IPaintCandles
{
    private readonly IGraphPane _graph;
    private readonly Queue<(int bullColor, int bearColor, int opacity)> _colors = new();

    public PaintCandles(IGraphPane graph)
    {
        _graph = graph;

        _colors.Enqueue((-6684885, -54485, 0));
        _colors.Enqueue((-9579520, -2883584, 180));
        _colors.Enqueue((-12419072, -8388608, 200));
    }

    public void Candles(ISecurity security, string name) => DrawCandles(security, name, false, _colors.Dequeue());

    public void Trades(ISecurity security) => DrawCandles(security, security.Symbol, true, (0, 0, 99999));

    private void DrawCandles(ISecurity security, string name, bool showTrades, (int bull, int bear, int opacity) colors)
    {
        var chart = _graph.AddList(name, name, security,
            CandleStyles.BAR_CANDLE, CandleFillStyle.All, showTrades,
            colors.bull, PaneSides.RIGHT);
        chart.AlternativeColor = colors.bear;
        chart.Opacity = colors.opacity;
        chart.Autoscaling = true;
        _graph.UpdatePrecision(PaneSides.RIGHT, security.Decimals);
    }
}