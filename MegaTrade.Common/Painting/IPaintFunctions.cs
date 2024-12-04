using TSLab.Script;

namespace MegaTrade.Common.Painting;

internal interface IPaintFunctions
{
    void Function(IList<double> values, string name);

    void Function(IList<double> values, string name, out Color usedColor);

    void Function(IList<double> values, string name, AnimalColor animalColor);

    void Function(IList<double> values, string name, AnimalColor animalColor, out Color usedColor);

    void Function(IList<double> values, string name, Color color);

    void Function(IList<double> values, string name, Color color, out Color usedColor);
}