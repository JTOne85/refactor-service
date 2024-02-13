using System;
using ThirdParty;
using System.Runtime.Caching;

namespace Adv
{
    public interface ICacheManager
    {
        Advertisement Get(string name);
        void Set(string cacheName, Advertisement adv, DateTimeOffset expirationTime);
        void SetCache(string cacheName, Advertisement adv);
        Advertisement GetCache(string name);

    }
    public abstract class ICacheManagerBase : ICacheManager
    {
        public abstract Advertisement Get(string name);

        public abstract Advertisement GetCache(string name);

        public abstract void Set(string cacheName, Advertisement adv, DateTimeOffset expirationTime);

        public abstract void SetCache(string cacheName, Advertisement adv);
    }

    public class CacheManager : ICacheManagerBase
    {
        private static MemoryCache Cache { get; set; }

        public override Advertisement GetCache(string name)
        {
            return Get(name);
        }
        public override void SetCache(string cacheName, Advertisement adv)
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
