using System;
using System.Web;
using System.Web.Caching;

public static class CacheService
{
    public static T GetOrSet<T>(string key, TimeSpan duration, Func<T> acquire)
    {
        var cache = HttpRuntime.Cache;
        var item = cache[key];

        if (item == null)
        {
            var value = acquire();
            cache.Insert(
                key,
                value,
                null,
                DateTime.UtcNow.Add(duration),
                Cache.NoSlidingExpiration);

            return value;
        }

        return (T)item;
    }
}
