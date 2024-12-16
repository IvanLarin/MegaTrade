using TSLab.Script;

namespace MegaTrade.Draw;

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

    IPaint FunctionWithoutZeroes(IList<double> values, string name);

    IPaint FunctionWithoutZeroes(IList<double> values, string name, out Color usedColor);

    IPaint FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor);

    IPaint FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor, out Color usedColor);

    IPaint FunctionWithoutZeroes(IList<double> values, string name, Color color);

    IPaint FunctionWithoutZeroes(IList<double> values, string name, Color color, out Color usedColor);

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

    IPaint Bound(params double[] bounds);

    IPaint BoundOfMin(params IList<double>[] bounds);

    IPaint BoundOfMax(params IList<double>[] bounds);

    IPaint BoundOfMajorMax(params IList<double>[] bounds);

    IPaint BoundOfMajorMin(params IList<double>[] bounds);

    IPaint BoundOfMajorMax(double majorityPercent, params IList<double>[] bounds);

    IPaint BoundOfMajorMin(double majorityPercent, params IList<double>[] bounds);

    IPaint DecimalPlaces(int count);
}