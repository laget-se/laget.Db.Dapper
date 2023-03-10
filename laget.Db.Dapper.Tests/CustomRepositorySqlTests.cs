using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using System;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class CustomRepositorySqlTests : IClassFixture<RepositoryFixture<AccountModel>>
    {
        private readonly TestRepository<AccountModel> _repository;

        public CustomRepositorySqlTests(RepositoryFixture<AccountModel> fixture)
        {
            _repository = fixture.Repository;
        }

        [Fact]
        public void GetInsertQueryGeneratesSql()
        {
            var model = new AccountModel();

            var query = _repository.ExposedGetInsertQuery(model);
            dynamic parameters = query.parameters;

            Assert.Equal("INSERT INTO [AccountModel] ([FirstName],[LastName],[Email]) OUTPUT INSERTED.[intAccountId] VALUES (@FirstName,@LastName,@Email)", query.sql);
            Assert.Equal(model.FirstName, parameters.FirstName);
            Assert.Equal(model.LastName, parameters.LastName);
            Assert.Equal(model.Email, parameters.Email);

            Assert.Equal(1, model.Id);
            Assert.Equal(DateTime.Now.AddMonths(-1).ToShortDateString(), model.CreatedAt.ToShortDateString());
            Assert.Equal(DateTime.Now.ToShortDateString(), model.UpdatedAt.ToShortDateString());
        }
    }
}