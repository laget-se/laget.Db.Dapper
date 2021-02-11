using System;
using Microsoft.Extensions.Caching.Memory;

namespace laget.Db.Dapper
{
    public interface IDapperDefaultProvider
    {
        MemoryCacheOptions CacheOptions { get; }
        string ConnectionString { get; }
    }

    public class DapperDefaultProvider : IDapperDefaultProvider
    {
        public MemoryCacheOptions CacheOptions { get; }
        public string ConnectionString { get; }

        public DapperDefaultProvider(string connectionString)
            : this(connectionString, new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(5)
            })
        {
        }

        public DapperDefaultProvider(string connectionString, MemoryCacheOptions cacheOptions)
        {
            ConnectionString = connectionString;
            CacheOptions = cacheOptions;
        }
    }
}
