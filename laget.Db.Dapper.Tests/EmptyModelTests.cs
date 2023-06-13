using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class EmptyModelTests : IClassFixture<RepositoryFixture<EmptyModel>>
    {
        private readonly TestRepository<EmptyModel> _repository;

        public EmptyModelTests(RepositoryFixture<EmptyModel> fixture)
        {
            _repository = fixture.Repository;
        }

        [Fact]
        public void GetInsertQueryGeneratesSql()
        {
            var model = new EmptyModel();

            var query = _repository.ExposedGetInsertQuery(model);
            var parameters = query.parameters;

            Assert.Equal("INSERT INTO [Table] OUTPUT INSERTED.[Id] DEFAULT VALUES", query.sql);
            Assert.Null(parameters);
        }
    }
}
