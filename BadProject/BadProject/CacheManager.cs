using System;
using ThirdParty;
using System.Runtime.Caching;

namespace Adv
{
    public abstract class ICacheManager
    {
        public abstract Advertisement Get(string name);
        public abstract void Set(string cacheName, Advertisement adv, DateTimeOffset expirationTime);
    }

    public class CacheManager : ICacheManager
    {
        private static MemoryCache Cache { get; set; }

        public Advertisement GetCache(string name)
        {
            return Get(name);
        }
        public void SetCache(string cacheName, Advertisement adv)
        {
            Set(cacheName, adv, DateTimeOffset.UtcNow);
        }

        public override Advertisement Get(string name)
        {
            return (Advertisement)Cache.Get(name);
        }

        public override void Set(string cacheName, Advertisement adv, DateTimeOffset expirationTime)
        {
            Cache.Set(cacheName, adv, expirationTime);
        }
    }
}
