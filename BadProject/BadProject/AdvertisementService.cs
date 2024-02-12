using BadProject;
using System;
using ThirdParty;

namespace Adv
{
    public class AdvertisementService : IAdvertisementService
    {
        //private static MemoryCache cache = new MemoryCache("");
        //private static Queue<DateTime> errors = new Queue<DateTime>();

        private Object lockObj = new Object();
        // **************************************************************************************************
        // Loads Advertisement information by id
        // from cache or if not possible uses the "mainProvider" or if not possible uses the "backupProvider"
        // **************************************************************************************************
        // Detailed Logic:
        // 
        // 1. Tries to use cache (and retuns the data or goes to STEP2)
        //
        // 2. If the cache is empty it uses the NoSqlDataProvider (mainProvider), 
        //    in case of an error it retries it as many times as needed based on AppSettings
        //    (returns the data if possible or goes to STEP3)
        //
        // 3. If it can't retrive the data or the ErrorCount in the last hour is more than 10, 
        //    it uses the SqlDataProvider (backupProvider)

        private CacheManager _cacheManager;
        private ErrorManager _errorManager;
        private AdvProviderBuilder _advProviderBuilder;

        public AdvertisementService(CacheManager cacheManager, ErrorManager errorManager)
        {
            _cacheManager = cacheManager;
            _errorManager = errorManager;
            _advProviderBuilder = new AdvProviderBuilder(_cacheManager, _errorManager);
        }

        public Advertisement GetAdvertisement(string id)
        {
            Advertisement adv = null;

            lock (lockObj)
            {
                // Use Cache if available
                adv = _cacheManager.Get($"AdvKey_{id}");

                // Count HTTP error timestamps in the last hour
                while (_errorManager.ErrorCount > 20) _errorManager.Dequeue();

                _errorManager.ErrorCount = 0;
                foreach (var dat in _errorManager.Errors)
                {
                    if (dat > DateTime.Now.AddHours(-1))
                    {
                        _errorManager.ErrorCount++;
                    }
                }

                return _advProviderBuilder.BuildProvider(adv, _errorManager);
            }
        }
    }
}
