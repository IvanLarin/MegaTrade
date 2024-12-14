using TSLab.Script.Handlers;

namespace MegaTrade.Common.Extensions;

public static class ContextExtensions
{
    public static T[] GetOrCreateArray<T>(this IContext? context, int count) =>
        context?.GetArray<T>(count) ?? new T[count];

    public static T[] GetOrCreateArray<T>(this IContext? context, IReadOnlyList<T> source) =>
        context.GetOrCreateArray<T>(source.Count).FillFrom(source);

    public static T[] GetOrCreateArray<T>(this IContext? context, IList<T> source) =>
        context.GetOrCreateArray<T>(source.Count).FillFrom(source);

    public static T[] GetOrCreateArray<T>(this IContext? context, IEnumerable<T> source) =>
        context.GetOrCreateArray((IList<T>)source.ToList());
}