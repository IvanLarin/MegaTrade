using HashDepot;
using TSLab.DataSource;
using TSLab.Script;
using TSLab.Script.Handlers;

namespace MegaTrade.Common.Extensions;

public static class ListExtensions
{
    public static uint CacheKey(this IList<double> source)
    {
        var bytes = new byte[source.Count * sizeof(double)];
        Buffer.BlockCopy(source.ToArray(), 0, bytes, 0, bytes.Length);

        return MurmurHash3.Hash32(bytes, 777);
    }

    public static T[] ToPoolArray<T>(this IList<T> source, IContext? context) => context.GetOrCreateArray(source);

    public static IList<T> DecompressBy<T>(this IList<T> source, ISecurity bySecurity) where T : struct =>
        bySecurity.Decompress(source, DecompressMethodWithDef.Default);
}