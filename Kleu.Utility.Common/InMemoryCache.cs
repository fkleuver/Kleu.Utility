using System;
using System.Runtime.Caching;

namespace Kleu.Utility.Common
{
    public static class InMemory
    {
        private static readonly NamedLocker Locker = new NamedLocker();

        public static TItem GetCachedItem<TItem>(
            string cacheKey,
            Func<TItem> retrieveItemIfNotInCache,
            TimeSpan expiration,
            bool expirationIsAbsolute = false,
            bool isNotRemovable = false)
            where TItem : class
        {
            if (MemoryCache.Default.Get(cacheKey) is TItem cachedItem)
            {
                return cachedItem;
            }

            lock (Locker.GetLock(cacheKey))
            {
                cachedItem = MemoryCache.Default.Get(cacheKey) as TItem;

                if (cachedItem != null)
                {
                    return cachedItem;
                }

                var retrievedItem = retrieveItemIfNotInCache();

                var cip = new CacheItemPolicy();
                if (expirationIsAbsolute)
                {
                    cip.AbsoluteExpiration = DateTime.UtcNow.AddTicks(expiration.Ticks);
                }
                else
                {
                    cip.SlidingExpiration = expiration;
                }

                cip.Priority = isNotRemovable ? CacheItemPriority.NotRemovable : CacheItemPriority.Default;

                MemoryCache.Default.Set(cacheKey, retrievedItem, cip);
                return retrievedItem;
            }
        }

        public static void RemoveCachedItem(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
        }

        public static void ClearCache()
        {
            MemoryCache.Default.Dispose();
        }
    }
}
