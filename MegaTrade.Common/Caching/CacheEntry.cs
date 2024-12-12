namespace MegaTrade.Common.Caching;

public class CacheEntry<T>(string name, object[] dependencies, CacheKind kind) : ICacheEntry<T>
{
    public T Calculate(Func<T> calculator) => Cache.Get(name, calculator, dependencies, kind);
}