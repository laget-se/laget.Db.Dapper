using System.Threading.Tasks;
using laget.Db.Dapper.Exceptions;
using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class ReadOnlyRepositoryTests : IClassFixture<ReadOnlyRepositoryFixture<TestModel>>
    {
        readonly ReadOnlyTestRepository<TestModel> _repository;

        public ReadOnlyRepositoryTests(ReadOnlyRepositoryFixture<TestModel> fixture)
        {
            _repository = fixture.Repository;
        }

        [Fact]
        public void ShouldThrowExceptionOnInsert()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.Insert(new TestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(TestModel)}", exception.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnInsertAsync()
        {
            var exception = await Assert.ThrowsAsync<ReadOnlyException>(() => _repository.InsertAsync(new TestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(TestModel)}", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionOnUpdate()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.Update(new TestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(TestModel)}", exception.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnUpdateAsync()
        {
            var exception = await Assert.ThrowsAsync<ReadOnlyException>(() => _repository.UpdateAsync(new TestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(TestModel)}", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionOnDelete()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.Delete(new TestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(TestModel)}", exception.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnDeleteAsync()
        {
            var exception = await Assert.ThrowsAsync<ReadOnlyException>(() => _repository.DeleteAsync(new TestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(TestModel)}", exception.Message);
        }
    }
}
