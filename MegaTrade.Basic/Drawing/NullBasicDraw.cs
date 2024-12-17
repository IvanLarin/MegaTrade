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

    public IPaint Paint { get; } = new NullPaint();

    public IPaint Chart(string name) => Paint;
}