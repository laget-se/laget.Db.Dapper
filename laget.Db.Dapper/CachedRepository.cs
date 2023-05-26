using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace laget.Db.Dapper
{
    public class CachedRepository<TEntity> : Repository<TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        public CachedRepository(IDapperDefaultProvider provider)
            : base(provider)
        {
        }

        [Obsolete("This method has no more usages and will be removed in a future version, please use List() instead.")]
        public override IEnumerable<TEntity> Find() =>
            List();
        [Obsolete("This method has no more usages and will be removed in a future version, please use ListAsync() instead.")]
        public override async Task<IEnumerable<TEntity>> FindAsync() =>
            await ListAsync();
        [Obsolete("This method has no more usages and will be removed in a future version, please use Where(string conditions) instead.")]
        public override IEnumerable<TEntity> Find(string where) =>
            Where(where);
        [Obsolete("This method has no more usages and will be removed in a future version, please use WhereAsync(string conditions) instead.")]
        public override async Task<IEnumerable<TEntity>> FindAsync(string where) =>
            await WhereAsync(where);

        public override IEnumerable<TEntity> List()
        {
            var cacheKey = $"{CachePrefix}";

            return Cache.GetOrCreate(cacheKey, entry => base.List());
        }

        public override async Task<IEnumerable<TEntity>> ListAsync()
        {
            var cacheKey = $"{CachePrefix}";

            return await Cache.GetOrCreate(cacheKey, entry => base.ListAsync());
        }

        public override IEnumerable<TEntity> Where(string conditions)
        {
            var cacheKey = $"{CachePrefix}_{conditions.GetHashCode()}";

            return Cache.GetOrCreate(cacheKey, entry => base.Where(conditions));
        }

        public override async Task<IEnumerable<TEntity>> WhereAsync(string conditions)
        {
            var cacheKey = $"{CachePrefix}_{conditions.GetHashCode()}";

            return await Cache.GetOrCreate(cacheKey, entry => base.WhereAsync(conditions));
        }

        public override TEntity Get(int id)
        {
            var cacheKey = $"{CachePrefix}_Id_{id}";

            return Cache.GetOrCreate(cacheKey, entry => base.Get(id));
        }

        public override async Task<TEntity> GetAsync(int id)
        {
            var cacheKey = $"{CachePrefix}_Id_{id}";

            return await Cache.GetOrCreate(cacheKey, entry => base.GetAsync(id));
        }

        public override IEnumerable<TEntity> Get(int[] ids)
        {
            var cacheKey = $"{CachePrefix}_Ids_{string.Join(string.Empty, ids).GetHashCode()}";

            return Cache.GetOrCreate(cacheKey, entry => base.Get(ids));
        }

        public override async Task<IEnumerable<TEntity>> GetAsync(int[] ids)
        {
            var cacheKey = $"{CachePrefix}_Ids_{string.Join(string.Empty, ids).GetHashCode()}";

            return await Cache.GetOrCreate(cacheKey, entry => base.GetAsync(ids));
        }

        public override TEntity Insert(TEntity entity)
        {
            var e = base.Insert(entity);
            var cacheKey = $"{CachePrefix}_Id_{e.Id}";

            return Cache.GetOrCreate(cacheKey, entry => e);
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            var e = await base.InsertAsync(entity);
            var cacheKey = $"{CachePrefix}_Id_{e.Id}";

            return Cache.GetOrCreate(cacheKey, entry => e);
        }

        public override TEntity Update(TEntity entity)
        {
            var e = base.Update(entity);
            var cacheKey = $"{CachePrefix}_Id_{e.Id}";

            return Cache.GetOrCreate(cacheKey, entry => e);
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var e = await base.UpdateAsync(entity);
            var cacheKey = $"{CachePrefix}_Id_{e.Id}";

            return Cache.GetOrCreate(cacheKey, entry => e);
        }

        public override void Delete(TEntity entity)
        {
            var cacheKey = $"{CachePrefix}_Id_{entity.Id}";
            Cache.Remove(cacheKey);

            base.Delete(entity);
        }

        public override async Task DeleteAsync(TEntity entity)
        {
            var cacheKey = $"{CachePrefix}_Id_{entity.Id}";
            Cache.Remove(cacheKey);

            await base.DeleteAsync(entity);
        }
    }
}
