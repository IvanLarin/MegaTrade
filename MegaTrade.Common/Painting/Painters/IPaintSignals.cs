using TSLab.Script;

namespace MegaTrade.Common.Painting.Painters;

internal interface IPaintSignals
{
    void Signal(IList<bool> values, string name);

    void Signal(IList<bool> values, string name, out Color usedColor);

    void Signal(IList<bool> values, string name, AnimalColor animalColor);

    void Signal(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor);

    void Signal(IList<bool> values, string name, Color color);

    void Signal(IList<bool> values, string name, Color color, out Color usedColor);
}