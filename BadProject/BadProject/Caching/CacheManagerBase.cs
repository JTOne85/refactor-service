using System;
using ThirdParty;

namespace BadProject.Caching
{
    public abstract class CacheManagerBase : ICacheManager
    {
        public abstract Advertisement Get(string name);

        public abstract Advertisement GetCache(string name);

        public abstract void Set(string cacheName, Advertisement adv, DateTimeOffset expirationTime);

        public abstract void SetCache(string cacheName, Advertisement adv);
    }
}
