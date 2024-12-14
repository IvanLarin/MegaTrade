using HashDepot;
using MegaTrade.Common.Caching;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Extensions;

public static class ListExtensions
{
    public static uint CacheKey<T>(this IList<T> source)
    {
        var bytes = new byte[source.Count * sizeof(double)];
        Buffer.BlockCopy(source.ToArray(), 0, bytes, 0, bytes.Length);

        return MurmurHash3.Hash32(bytes, 777);
    }

    public static T[] ToPoolArray<T>(this IList<T> source, IContext? context) => context.GetOrCreateArray(source);

    public static IList<T> DecompressBy<T>(this IList<T> source, ISecurity byTimeframe) where T : struct =>
        Cache
            .Entry<IList<T>>("Decompressed", CacheKind.Memory, [source.CacheKey(), byTimeframe.CacheKey()])
            .Calculate(() => byTimeframe.Decompress(source, DecompressMethodWithDef.Default));
}