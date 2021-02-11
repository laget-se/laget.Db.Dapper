using System;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class ProviderTests
    {
        private static string ConnectionString => "Server=mssql0.example.com;Database=database;User Id=myDBReader;Password=D1fficultP%40ssw0rd;";

        [Fact]
        public void ShouldReturnCorrectConnectionString()
        {
            var provider = new Mock<DapperDefaultProvider>(ConnectionString).Object;

            var actual = provider.ConnectionString;

            Assert.Equal(ConnectionString, actual);
        }

        [Fact]
        public void ShouldReturnAllCorrectValues()
        {
            const double compactionPercentage = 0.25;
            var expirationScanFrequency = TimeSpan.FromMinutes(1);
            const int sizeLimit = 1024;

            var provider = new Mock<DapperDefaultProvider>(ConnectionString, new MemoryCacheOptions
            {
                CompactionPercentage = compactionPercentage,
                ExpirationScanFrequency = expirationScanFrequency,
                SizeLimit = sizeLimit
            }).Object;

            Assert.Equal(ConnectionString, provider.ConnectionString);
            Assert.Equal(compactionPercentage, provider.CacheOptions.CompactionPercentage);
            Assert.Equal(expirationScanFrequency, provider.CacheOptions.ExpirationScanFrequency);
            Assert.Equal(sizeLimit, provider.CacheOptions.SizeLimit);
        }
    }
}
