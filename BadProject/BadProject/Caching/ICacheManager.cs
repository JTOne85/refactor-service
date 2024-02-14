using System;
using ThirdParty;

namespace BadProject.Caching
{
    public interface ICacheManager
    {
        Advertisement Get(string name);
        void Set(string cacheName, Advertisement adv, DateTimeOffset expirationTime);
        void SetCache(string cacheName, Advertisement adv);
        Advertisement GetCache(string name);

    }
}
