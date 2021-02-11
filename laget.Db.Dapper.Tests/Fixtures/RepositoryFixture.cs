using System;

namespace laget.Db.Dapper.Tests.Fixtures
{
    public class RepositoryFixture<TEntity> : IDisposable where TEntity : Entity
    {
        public TestRepository<TEntity> Repository { get; private set; }

        public RepositoryFixture()
        {
            Repository = new TestRepository<TEntity>();
        }

        public void Dispose()
        {
            Repository = null;
        }
    }

    public class TestRepository<TEntity> : Repository<TEntity> where TEntity : Entity
    {
        public TestRepository() : base(new DapperDefaultProvider("Server=mssql0.example.com;Database=database;User Id=myDBReader;Password=D1fficultP%40ssw0rd;")) { }

        public (string sql, object parameters) ExposedGetInsertQuery(TEntity entity) => GetInsertQuery(entity);

        public (string sql, object parameters) ExposedGetUpdateQuery(TEntity entity) => GetUpdateQuery(entity);

        public (string sql, object parameters) ExposedGetDeleteQuery(TEntity entity) => GetDeleteQuery(entity);
    }
}
