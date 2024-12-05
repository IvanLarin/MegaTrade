using TSLab.Script;

namespace MegaTrade.Common.Painting;

public interface IPaint
{
    IPaint Candles(ISecurity security, string? name = null);

    IPaint Trades(ISecurity security);

    IPaint Function(IList<double> values, string name);

    IPaint Function(IList<double> values, string name, out Color usedColor);

    IPaint Function(IList<double> values, string name, AnimalColor animalColor);

    IPaint Function(IList<double> values, string name, AnimalColor animalColor, out Color usedColor);

    IPaint Function(IList<double> values, string name, Color color);

    IPaint Function(IList<double> values, string name, Color color, out Color usedColor);

    IPaint Histogram(IList<double> values, string name);

    IPaint Histogram(IList<double> values, string name, out Color usedColor);

    IPaint Histogram(IList<double> values, string name, AnimalColor animalColor);

    IPaint Histogram(IList<double> values, string name, AnimalColor animalColor, out Color usedColor);

    IPaint Histogram(IList<double> values, string name, Color color);

    IPaint Histogram(IList<double> values, string name, Color color, out Color usedColor);

    IPaint Signal(IList<bool> values, string name);

    IPaint Signal(IList<bool> values, string name, out Color usedColor);

    IPaint Signal(IList<bool> values, string name, AnimalColor animalColor);

    IPaint Signal(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor);

    IPaint Signal(IList<bool> values, string name, Color color);

    IPaint Signal(IList<bool> values, string name, Color color, out Color usedColor);

    IPaint Level(double value, string name, Color color);

    IPaint Level(double value, string name, Color color, out Color usedColor);

    IPaint Bound(double bound);
}