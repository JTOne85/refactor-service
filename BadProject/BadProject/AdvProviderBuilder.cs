﻿using Adv;
using System;
using System.Configuration;
using System.Threading;
using ThirdParty;

namespace BadProject
{
    public class AdvProviderBuilder
    {
        private ErrorManager _errorManager;
        private CacheManager _cacheManager;

        public CacheManager CacheManager
        {
            get => _cacheManager;
            set
            {
                if (_cacheManager == null) _cacheManager = new CacheManager();
            }
        }

        public ErrorManager ErrorManager
        {
            get => _errorManager;
            set
            {
                if (_errorManager == null) _errorManager = new ErrorManager();
            }
        }

        public AdvProviderBuilder(CacheManager cacheManager, ErrorManager errorManager)
        {
            CacheManager = cacheManager;
            ErrorManager = errorManager;
        }

        public Advertisement BuildProvider(Advertisement adv, ErrorManager errorManager)
        {
            ErrorManager = errorManager;
            if ((adv == null) && (errorManager.ErrorCount < 10))
            {
                int retry = 0;
                do
                {
                    retry++;
                    try
                    {
                        adv = CreateAdvProvider(adv.WebId);
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        EnqueueError(DateTime.Now);
                    }
                } while ((adv == null) && (retry < int.Parse(ConfigurationManager.AppSettings["RetryCount"])));
            }

            // if needed try to use Backup provider
            if (adv == null)
            {
                adv = CreateAdvProvider(adv.WebId, true);
            }

            if (adv != null)
            {
                CacheManager.SetCache($"AdvKey_{adv.WebId}", adv);
            }

            return adv;
        }


        private Advertisement CreateAdvProvider(string id, bool? useBackupProvider = false)
        {
            if (useBackupProvider.Value)
            {
                var dataProvider = new NoSqlAdvProvider();
                return dataProvider.GetAdv(id);
            }
            else return SQLAdvProvider.GetAdv(id);
        }

        private void EnqueueError(DateTime errorTimeStamp)
        {
            ErrorManager.Enqueue(errorTimeStamp);
        }


    }
}
