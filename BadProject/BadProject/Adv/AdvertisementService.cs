using BadProject.Caching;
using BadProject.Errors;
using System;
using ThirdParty;

namespace Adv
{
    public class AdvertisementService : IAdvertisementService
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
