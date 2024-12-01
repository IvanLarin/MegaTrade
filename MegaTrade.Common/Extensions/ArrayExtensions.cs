namespace MegaTrade.Common.Extensions;

public static class ArrayExtensions
{
    public static T[] FillFrom<T>(this T[] array, IEnumerable<T> source)
    {
        var index = 0;
        foreach (var item in source)
            array[index++] = item;

        return array;
    }
}