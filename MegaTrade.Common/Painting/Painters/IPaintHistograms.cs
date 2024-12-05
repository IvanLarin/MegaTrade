using TSLab.Script;

namespace MegaTrade.Common.Painting.Painters;

internal interface IPaintHistograms
{
    void Histogram(IList<double> values, string name);

    void Histogram(IList<double> values, string name, out Color usedColor);

    void Histogram(IList<double> values, string name, AnimalColor animalColor);

    void Histogram(IList<double> values, string name, AnimalColor animalColor, out Color usedColor);

    void Histogram(IList<double> values, string name, Color color);

    void Histogram(IList<double> values, string name, Color color, out Color usedColor);
}