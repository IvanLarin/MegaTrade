using HashDepot;
using TSLab.Script;

namespace MegaTrade.Common.Caching;

public static class Extensions
{
    public static uint CacheKey<T>(this IList<T> source)
    {
        var bytes = new byte[source.Count * sizeof(double)];
        Buffer.BlockCopy(source.ToArray(), 0, bytes, 0, bytes.Length);

        return MurmurHash3.Hash32(bytes, 777);
    }

    public static uint CacheKey(this ISecurity security) =>
        security.Bars.Select(x => x.Close).ToArray().CacheKey();
}