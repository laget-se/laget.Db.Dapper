using System.Threading.Tasks;
using laget.Db.Dapper.Exceptions;
using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class ReadOnlyRepositoryTests : IClassFixture<ReadOnlyRepositoryFixture<ReadOnlyTestModel>>
    {
        readonly ReadOnlyTestRepository<ReadOnlyTestModel> _repository;

        public ReadOnlyRepositoryTests(ReadOnlyRepositoryFixture<ReadOnlyTestModel> fixture)
        {
            _repository = fixture.Repository;
        }

        [Fact]
        public void ShouldThrowExceptionOnInsert()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.Insert(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnInsertAsync()
        {
            var exception = await Assert.ThrowsAsync<ReadOnlyException>(() => _repository.InsertAsync(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionOnUpdate()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.Update(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnUpdateAsync()
        {
            var exception = await Assert.ThrowsAsync<ReadOnlyException>(() => _repository.UpdateAsync(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionOnDelete()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.Delete(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnDeleteAsync()
        {
            var exception = await Assert.ThrowsAsync<ReadOnlyException>(() => _repository.DeleteAsync(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }


        [Fact]
        public void ShouldThrowExceptionOnGetInsertQuery()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.ExposedGetInsertQuery(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionOnGetUpdateQuery()
        {
            var exception = Assert.Throws<ReadOnlyException>(() => _repository.ExposedGetUpdateQuery(new ReadOnlyTestModel()));
            Assert.Equal($"We're not allowing writes to a read-only repository: {nameof(ReadOnlyTestModel)}", exception.Message);
        }
    }
}
