using TSLab.Script.Handlers;

namespace MegaTrade.Common.Extensions;

public static class ContextExtensions
{
    public static T[] GetOrCreateArray<T>(this IContext? ctx, int count) => ctx?.GetArray<T>(count) ?? new T[count];

    public static T[] GetOrCreateArray<T>(this IContext? ctx, IReadOnlyList<T> source) =>
        ctx.GetOrCreateArray<T>(source.Count).FillFrom(source);

    public static T[] GetOrCreateArray<T>(this IContext? ctx, IList<T> source) =>
        ctx.GetOrCreateArray<T>(source.Count).FillFrom(source);
}