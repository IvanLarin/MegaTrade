using TSLab.Script.Handlers;

namespace MegaTrade.Common.Caching;

public class CacheEntry<T>(string name, object[] dependencies, IContext? ctx, CacheKind kind) : ICacheEntry<T>
{
    public T Calculate(Func<T> calculator) => Cache.Get(
        name,
        calculator,
        dependencies,
        ctx, kind);
}