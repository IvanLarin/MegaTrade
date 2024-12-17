using TSLab.DataSource;
using TSLab.Script;

namespace MegaTrade.Basic.Extensions;

/// <summary>
///     Класс, содержащий методы расширения для работы с торговыми данными.
/// </summary>
public static class TradeExtensions
{
    /// <summary>
    ///     Преобразует индекс бара основного таймфрейма в индекс бара целевого таймфрейма, полученного в результате сжатия
    ///     основного таймфрейма.
    /// </summary>
    public static int To(this int basicTimeframeBarIndex, ISecurity toTimeframe) =>
        new BarIndexToTimeframe
        {
            BarIndex = basicTimeframeBarIndex,
            ToTimeframe = toTimeframe
        }.Result;

    /// <summary>
    ///     Разжимает данные из указанного таймфрейма к основному таймфрейму.
    /// </summary>
    public static IList<T> DecompressFrom<T>(this IList<T> source, ISecurity fromTimeframe) where T : struct =>
        fromTimeframe.Decompress(source, DecompressMethodWithDef.Default);
}