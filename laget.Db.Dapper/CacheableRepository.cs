using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace laget.Db.Dapper
{
    public class CacheableRepository<TEntity> : Repository<TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        protected readonly IMemoryCache Cache;
        protected readonly Cache Options;

        public CacheableRepository(string connectionString, Cache cache)
            : base(connectionString)
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(1)
            });
            Options = cache;
        }


        public override IEnumerable<TEntity> Find()
        {
            var item = CacheGet<IEnumerable<TEntity>>("Find");
            if (item != null)
            {
                return item;
            }

            var result = base.Find();

            CacheAdd("Find", result);

            return result;
        }

        public override async Task<IEnumerable<TEntity>> FindAsync()
        {
            var item = CacheGet<IEnumerable<TEntity>>("Find");
            if (item != null)
            {
                return await Task.FromResult(item);
            }

            var result = base.FindAsync();

            CacheAdd("Find", result.Result);

            return await result;
        }

        public override TEntity Get(int id)
        {
            var item = CacheGet<TEntity>(id.ToString());
            if (item != null)
            {
                return item;
            }

            var result = base.Get(id);

            CacheAdd(id.ToString(), result);

            return result;
        }

        //public override async Task<TEntity> GetAsync(int id)
        //{
        //    if (!string.IsNullOrWhiteSpace(Options.KeyPrefix))
        //    {
        //        var item = CacheGet<TEntity>(id.ToString());
        //        if (item != null)
        //        {
        //            return await Task.FromResult(item);
        //        }
        //    }

        //    var response = base.GetAsync(id);

        //    if (!string.IsNullOrWhiteSpace(Options.KeyPrefix))
        //    {
        //        CacheAdd(id.ToString(), response.Result);
        //    }

        //    return await response;
        //}

        //public override int Insert(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public override async Task<int> InsertAsync(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void Update(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public override async Task UpdateAsync(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}


        protected TZ CacheGet<TZ>(string key)
        {
            return Cache.Get<TZ>($"{Options.KeyPrefix}_{key}");
        }

        protected void CacheAdd<TZ>(string key, TZ item)
        {
            Cache.Set($"{Options.KeyPrefix}_{key}", item,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                    AbsoluteExpirationRelativeToNow = Options.Expiration,
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = Options.Expiration
                });
        }
    }
}
