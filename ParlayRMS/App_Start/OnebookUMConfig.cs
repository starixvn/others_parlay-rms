namespace ParlayRMS
{
    using System;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using ONEbook.UM;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Transport;

    public static class OnebookUMConfig
    {
        public static void Register()
        {
            var umConfig = new UMClientConfiguration().Initialize();

            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new MemoryUmCacheService())
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new HttpContextHelper());
        }
    }


    public class MemoryUmCacheService : IUMCacheService
    {

        private static readonly ICache CacheInstance = new MemoryCache(
            new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromSeconds(5),
                CompactionPercentage = 0.15,
                CompactOnMemoryPressure = true
            }
        );

        public T Get<T>(string key)
        {
            CacheInstance.TryGet(key, out T result);
            return result;
        } 

        public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
            => CacheInstance.Set(key, value, new CacheItemOptions { RelativeExpiration = TimeSpan.FromMinutes(1) });

        public void Remove(string key) => CacheInstance.Remove(key);
    }

}
