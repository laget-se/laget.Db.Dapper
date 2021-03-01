using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class CachedRepositoryTests : IClassFixture<CachedRepositoryFixture<AccountModel>>
    {
        private readonly CachedTestRepository<AccountModel> _repository;

        public CachedRepositoryTests(CachedRepositoryFixture<AccountModel> fixture)
        {
            _repository = fixture.Repository;

            _repository.Insert(new AccountModel());
            _repository.Insert(new AccountModel());
            _repository.Insert(new AccountModel());
        }


        [Fact]
        public void ShouldThrowExceptionOnInsert()
        {
            var model = new AccountModel
            {
                FirstName = "John",
            };

            _repository.Insert(model);

            Assert.Equal(4, _repository.ExposedConcurrentDictionary.Count);
        }
    }
}
