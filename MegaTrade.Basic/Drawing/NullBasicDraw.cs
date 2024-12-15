using MegaTrade.Draw;

namespace MegaTrade.Basic.Drawing;

internal class NullBasicDraw : IBasicDraw
{
    public void Draw()
    {
    }

    public void PushSignals()
    {
    }

    private readonly IPaint _paint = new NullPaint();

    public IPaint AddPaint(string name) => _paint;

    public IPaint Paint => _paint;
}