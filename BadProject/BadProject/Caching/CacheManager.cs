using System;
using ThirdParty;
using System.Runtime.Caching;

namespace BadProject.Caching
{

    public class CacheManager : CacheManagerBase
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
