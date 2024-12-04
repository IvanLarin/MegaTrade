using TSLab.Script;

namespace MegaTrade.Common.Painting;

public interface IPaintHistogram
{
    void Histogram(IList<bool> values, string name);

    void Histogram(IList<bool> values, string name, out Color usedColor);

    void Histogram(IList<bool> values, string name, AnimalColor animalColor);

    void Histogram(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor);

    void Histogram(IList<bool> values, string name, Color color);

    void Histogram(IList<bool> values, string name, Color color, out Color usedColor);

    void Signal(IList<bool> values, string name);

    void Signal(IList<bool> values, string name, out Color usedColor);

    void Signal(IList<bool> values, string name, AnimalColor animalColor);

    void Signal(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor);

    void Signal(IList<bool> values, string name, Color color);

    void Signal(IList<bool> values, string name, Color color, out Color usedColor);
}