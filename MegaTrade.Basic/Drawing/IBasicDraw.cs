using MegaTrade.Draw;

namespace MegaTrade.Basic.Drawing;

internal interface IBasicDraw : ICharter
{
    void Draw();

    void PushSignals();

    IPaint Paint { get; }
}