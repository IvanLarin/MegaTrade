namespace MegaTrade.Common.Caching;

public interface ICacheEntry<T>
{
    T Calculate(Func<T> calculator);
}