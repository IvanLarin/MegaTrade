using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MegaTrade.Common.Caching;

public static class Cache
{
    private static readonly ConcurrentDictionary<string, object> Locks = new();

    public static T Get<T>(string name, Func<T> calculate, object[] dependencies, CacheKind kind)
    {
        var key = MakeKey(name, dependencies);

        if (Load(out var data, kind)) return data!;

        var lockObject = Locks.GetOrAdd(key, _ => new object());

        lock (lockObject)
        {
            if (Load(out var data2, kind)) return data2!;

            var result = calculate();

            Save(result, kind);

            return result;
        }

        bool Load(out T? output, CacheKind theKind)
        {
            output = default;

            if (theKind == CacheKind.Memory)
            {
                var fromCache = Local.Context?.LoadObject(key);
                if (fromCache is not T dataFromCache) return false;

                output = dataFromCache;
                return true;
            }

            if (theKind == CacheKind.Disk)
            {
                var fromCache = Local.Context?.LoadObject(key, true);
                if (fromCache is not T dataFromCache) return false;

                output = dataFromCache;
                return true;
            }

            if (Load(out var data3, CacheKind.Memory))
            {
                output = data3;
                return true;
            }

            if (Load(out var data4, CacheKind.Disk))
            {
                Save(data4!, CacheKind.Memory);
                output = data4;
                return true;
            }

            return false;
        }

        void Save(T value, CacheKind theKind)
        {
            switch (theKind)
            {
                case CacheKind.Memory:
                    Local.Context?.StoreObject(key, value);
                    break;
                case CacheKind.Disk:
                    Local.Context?.StoreObject(key, value, true);
                    break;
                case CacheKind.DiskAndMemory:
                    Local.Context?.StoreObject(key, value);
                    Local.Context?.StoreObject(key, value, true);
                    break;
                default: throw new NotImplementedException();
            }
        }
    }

    private static string MakeKey(string name, object[] dependencies) =>
        $"{name}: {JsonConvert.SerializeObject(dependencies)}";

    public static ICacheEntry<T> Entry<T>(string name, CacheKind kind, object[] dependencies) =>
        new CacheEntry<T>(name, dependencies, kind);
}