using MegaTrade.Draw;

namespace MegaTrade.Basic.Drawing;

internal interface IBasicDraw
{
    void Draw();

    void PushSignals();

    IPaint AddPaint(string name);

    IPaint Paint { get; }
}