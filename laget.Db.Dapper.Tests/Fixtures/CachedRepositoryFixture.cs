using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace laget.Db.Dapper.Tests.Fixtures
{
    public class CachedRepositoryFixture<TEntity> : IDisposable where TEntity : Entity
    {
        public CachedTestRepository<TEntity> Repository { get; private set; }

        public CachedRepositoryFixture()
        {
            Repository = new CachedTestRepository<TEntity>();
        }

        public void Dispose()
        {
            Repository = null;
        }
    }

    public class CachedTestRepository<TEntity> : CachedRepository<TEntity> where TEntity : Entity
    {
        private readonly ConcurrentDictionary<int, TEntity> _dictionary;

        public CachedTestRepository()
            : base(new DapperDefaultProvider("Server=mssql0.example.com;Database=database;User Id=myDBReader;Password=D1fficultP%40ssw0rd;"))
        {
            _dictionary = new ConcurrentDictionary<int, TEntity>();
        }

        public override IEnumerable<TEntity> List()
        {
            return _dictionary.Select(x => x.Value);
        }

        public override async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await Task.Run(() =>
            {
                return _dictionary.Select(x => x.Value);
            });
        }

        public override TEntity Get(int id)
        {
            return _dictionary.FirstOrDefault(x => x.Key == id).Value;
        }

        public override async Task<TEntity> GetAsync(int id)
        {
            return await Task.Run(() =>
            {
                return _dictionary.FirstOrDefault(x => x.Key == id).Value;
            });
        }

        public override TEntity Insert(TEntity entity)
        {
            entity.Id = (_dictionary.Count + 1);
            return _dictionary.GetOrAdd(entity.Id, entity);
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            entity.Id = (_dictionary.Count + 1);
            return await Task.Run(() => _dictionary.GetOrAdd(entity.Id, entity));
        }

        public override TEntity Update(TEntity entity)
        {
            _dictionary.TryUpdate(entity.Id, entity, entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                _dictionary.TryUpdate(entity.Id, entity, entity);
                return entity;
            });
        }

        public override void Delete(TEntity entity)
        {
            _dictionary.TryRemove(entity.Id, out _);
        }

        public override async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                _dictionary.TryRemove(entity.Id, out _);
            });
        }

        public (string sql, object parameters) ExposedGetInsertQuery(TEntity entity) => GetInsertQuery(entity);

        public (string sql, object parameters) ExposedGetUpdateQuery(TEntity entity) => GetUpdateQuery(entity);

        public (string sql, object parameters) ExposedGetDeleteQuery(TEntity entity) => GetDeleteQuery(entity);

        public ConcurrentDictionary<int, TEntity> ExposedConcurrentDictionary => _dictionary;
    }
}
