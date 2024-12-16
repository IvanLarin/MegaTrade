using TSLab.Script.Handlers;

namespace MegaTrade.Common.Extensions;

public static class ContextExtensions
{
    public static T[] GetOrCreateArray<T>(this IContext? context, int count) =>
        context?.GetArray<T>(count) ?? new T[count];
}