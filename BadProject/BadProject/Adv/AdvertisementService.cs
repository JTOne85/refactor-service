using BadProject.Caching;
using BadProject.Errors;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using System.Threading;
using ThirdParty;

namespace Adv
{
    public class AdvertisementService
    {
        private object lockObj = new object();
        private ICacheManager _cacheManager;
        private IErrorManager _errorManager;
        private IAdvProviderBuilder _advProviderBuilder;

        public AdvertisementService(ICacheManager cacheManager, IErrorManager errorManager)
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
                adv = _cacheManager.Get($"AdvKey_{id}");

                while (_errorManager.ErrorCount > 20) _errorManager.Dequeue();


                // If Cache is empty and ErrorCount<10 then use HTTP provider
                if ((adv == null) && (errorCount < 10))
                {
                    int retry = 0;
                    do
                    {
                        retry++;
                        try
                        {
                            var dataProvider = new NoSqlAdvProvider();
                            adv = dataProvider.GetAdv(id);
                        }
                        catch
                        {
                            Thread.Sleep(1000);
                            errors.Enqueue(DateTime.Now); // Store HTTP error timestamp              
                        }
                    } while ((adv == null) && (retry < int.Parse(ConfigurationManager.AppSettings["RetryCount"])));


                    if (adv != null)
                    {
                        cache.Set($"AdvKey_{id}", adv, DateTimeOffset.Now.AddMinutes(5));
                    }
                }


                // if needed try to use Backup provider
                if (adv == null)
                {
                    adv = SQLAdvProvider.GetAdv(id);

                    if (adv != null)
                    {
                        cache.Set($"AdvKey_{id}", adv, DateTimeOffset.Now.AddMinutes(5));
                    }
                }
            }
            return adv;
        }
    }
}
