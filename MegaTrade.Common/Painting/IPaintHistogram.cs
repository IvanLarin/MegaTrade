using TSLab.Script;

namespace MegaTrade.Common.Painting;

public interface IPaintHistogram
{
    void Histogram(IList<bool> values, string name, AnimalColor animalColor = AnimalColor.Neutral);

    void Histogram(IList<bool> values, string name, Color color);

    void Signal(IList<bool> values, string name, AnimalColor animalColor = AnimalColor.Neutral);

    void Signal(IList<bool> values, string name, Color color);
}