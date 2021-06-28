using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace laget.Db.Dapper
{
    public class CachedRepository<TEntity> : Repository<TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        public CachedRepository(IDapperDefaultProvider provider)
            : base(provider)
        {
        }

        public override IEnumerable<TEntity> Find()
        {
            var cacheKey = $"{CachePrefix}";

            return Cache.GetOrCreate(cacheKey, entry => base.Find());
        }

        public override async Task<IEnumerable<TEntity>> FindAsync()
        {
            var cacheKey = $"{CachePrefix}";

            return await Cache.GetOrCreate(cacheKey, entry => base.FindAsync());
        }

        public override IEnumerable<TEntity> Find(string where)
        {
            var cacheKey = $"{CachePrefix}_{where.GetHashCode()}";

            return Cache.GetOrCreate(cacheKey, entry => base.Find());
        }

        public override async Task<IEnumerable<TEntity>> FindAsync(string where)
        {
            var cacheKey = $"{CachePrefix}_{where.GetHashCode()}";

            return await Cache.GetOrCreate(cacheKey, entry => base.FindAsync());
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
