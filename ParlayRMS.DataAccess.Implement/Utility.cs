namespace ParlayRMS.DataAccess.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Logging;

    internal static class Utility
    {
        private static ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

        public static string GetConnectionString(string connectionstringkey)
        {
            if (string.IsNullOrEmpty(connectionstringkey))
            {
                throw new ArgumentNullException("ConnectionStringKey is null or empty");
            }

            var cacheKey = "RMS.DataAccess.Implement." + connectionstringkey;

            var connectionString = cache.Get<string>(cacheKey);
            if (string.IsNullOrEmpty(connectionString))
            {
                var decryptedConnString = ConfigurationManager.ConnectionStrings[connectionstringkey].ConnectionString;
                connectionString = Cryptography.GetDeCryptConnectionString(decryptedConnString);
                cache.Set<string>(cacheKey, connectionString);
            }

            return connectionstringkey;
        }
    }
}