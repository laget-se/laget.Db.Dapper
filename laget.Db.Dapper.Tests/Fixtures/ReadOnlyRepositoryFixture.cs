using System;

namespace laget.Db.Dapper.Tests.Fixtures
{
    public class ReadOnlyRepositoryFixture<TEntity> : IDisposable where TEntity : ReadOnlyEntity
    {
        public ReadOnlyTestRepository<TEntity> Repository { get; private set; }

        public ReadOnlyRepositoryFixture()
        {
            Repository = new ReadOnlyTestRepository<TEntity>();
        }

        public void Dispose()
        {
            Repository = null;
        }
    }

    public class ReadOnlyTestRepository<TEntity> : ReadOnlyRepository<TEntity> where TEntity : ReadOnlyEntity
    {
        public ReadOnlyTestRepository() : base(string.Empty) { }

        public (string sql, object parameters) ExposedGetInsertQuery(TEntity entity) => GetInsertQuery(entity);

        public (string sql, object parameters) ExposedGetUpdateQuery(TEntity entity) => GetUpdateQuery(entity);

        public (string sql, object parameters) ExposedGetDeleteQuery(TEntity entity) => GetDeleteQuery(entity);
    }
}
