using TSLab.Script;

namespace MegaTrade.Basic;

/// <summary>
/// Настройки для <see cref="SystemBase"/>.
/// </summary>
public class Setup
{
    /// <summary>
    /// Получает ISecurity, который передаётся первым аргументом в метод Execute.
    /// </summary>
    public required ISecurity BasicTimeframe { get; init; }

    /// <summary>
    /// Получает массив номеров баров, раньше которых нельзя начинать торговлю.
    /// Наибольший из них будет ограничивать начало торговли.
    /// </summary>
    public int[] MinBarNumberLimits { get; init; } = [];
}