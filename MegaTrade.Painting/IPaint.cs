using TSLab.Script;

namespace MegaTrade.Draw;

/// <summary>
/// Предоставляет методы для рисования на своей панели графика.
/// <para>Перегрузки <c>Function</c> русуют линии.</para>
/// <para>Перегрузки <c>FunctionWithoutZeroes</c> рисуют линии, у которых не должны отображаются нули.</para>
/// <para>Перегрузки <c>Histogram</c> рисуют вертикальные полосы.</para>
/// <para>Перегрузки <c>Signal</c> рисуют полупрозрачные вертикальные полосы.
/// Благодаря полупрозрачности можно рисовать несколько <c>Signal</c> и цвета будут смешиваться для придания дополнительной семантики.</para>
/// <para>Перегрузки <c>Level</c> рисуют горизонтальные прямые.</para>
/// <para>Некоторые перегрузки через модификатор out возвращают цвет, который был использован для рисования, чтобы им можно было продолжить рисовать что-то ещё.</para>
/// <para>Если цвет не указан, то из палитры автоматически выбирается уникальный цвет. По умолчанию используется нейтральная палитра <see cref="AnimalColor.Neutral"/>.</para>
/// </summary>
public interface IPaint
{
    /// <summary>
    /// Рисует свечи.
    /// </summary>
    IPaint Candles(ISecurity security, string? name = null);

    /// <summary>
    /// Рисует сделки.
    /// </summary>
    IPaint Trades(ISecurity security);

    /// <summary>
    /// Рисует одну или несколько невидимых горизонтальных границ, чтобы видимые границы оси ординат оставались на своих местах во время прокрутки.
    /// </summary>
    IPaint Bound(params double[] bounds);

    /// <summary>
    /// Задаёт количество знаков после десятичной запятой на левой стороне панели графика.
    /// </summary>
    IPaint LeftDecimalPlaces(int count);

    /// <summary>
    /// Задаёт количество знаков после десятичной запятой на правой стороне панели графика.
    /// </summary>
    IPaint RightDecimalPlaces(int count);

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
}