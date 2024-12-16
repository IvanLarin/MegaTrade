using TSLab.Script;

namespace MegaTrade.Draw;

public class NullPaint : IPaint
{
    public IPaint Candles(ISecurity security, string? name = null) => this;

    public IPaint Trades(ISecurity security) => this;

    public IPaint Function(IList<double> values, string name) => this;

    public IPaint Function(IList<double> values, string name, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Function(IList<double> values, string name, AnimalColor animalColor) => this;

    public IPaint Function(IList<double> values, string name, AnimalColor animalColor, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Function(IList<double> values, string name, Color color) => this;

    public IPaint Function(IList<double> values, string name, Color color, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name) => this;

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor) => this;

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, AnimalColor animalColor, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, Color color) => this;

    public IPaint FunctionWithoutZeroes(IList<double> values, string name, Color color, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Histogram(IList<double> values, string name) => this;

    public IPaint Histogram(IList<double> values, string name, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, AnimalColor animalColor) => this;

    public IPaint Histogram(IList<double> values, string name, AnimalColor animalColor, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Histogram(IList<double> values, string name, Color color) => this;

    public IPaint Histogram(IList<double> values, string name, Color color, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Signal(IList<bool> values, string name) => this;

    public IPaint Signal(IList<bool> values, string name, out Color usedColor) => throw new NotImplementedException();

    public IPaint Signal(IList<bool> values, string name, AnimalColor animalColor) => this;

    public IPaint Signal(IList<bool> values, string name, AnimalColor animalColor, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Signal(IList<bool> values, string name, Color color) => this;

    public IPaint Signal(IList<bool> values, string name, Color color, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Level(double value, string name, Color color) => this;

    public IPaint Level(double value, string name, Color color, out Color usedColor)
    {
        usedColor = ScriptColors.Black;
        return this;
    }

    public IPaint Bound(params double[] bounds) => this;

    public IPaint BoundOfMin(params IList<double>[] bounds) => this;

    public IPaint BoundOfMax(params IList<double>[] bounds) => this;

    public IPaint BoundOfMajorMax(params IList<double>[] bounds) => this;

    public IPaint BoundOfMajorMin(params IList<double>[] bounds) => this;

    public IPaint BoundOfMajorMax(double majorityPercent, params IList<double>[] bounds) => this;

    public IPaint BoundOfMajorMin(double majorityPercent, params IList<double>[] bounds) => this;

    public IPaint DecimalPlaces(int count) => this;
}