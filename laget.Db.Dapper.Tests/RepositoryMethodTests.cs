using laget.Db.Dapper.Tests.Fixtures;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class RepositoryMethodTests
    {
        private readonly Mock<TestRepository<Models.TestModel>> _repository;

        public RepositoryMethodTests()
        {
            _repository = new Mock<TestRepository<Models.TestModel>> { CallBase = true };
        }

        [Fact]
        public void FindReturnsData()
        {
            _repository.Setup(x => x.List()).Returns(new List<Models.TestModel> { new Models.TestModel(), new Models.TestModel(), new Models.TestModel() });

            var models = _repository.Object.List().ToList();

            _repository.Verify(x => x.List(), Times.Once());

            Assert.Equal(3, models.Count);
        }

        [Fact]
        public async Task FindAsyncReturnsData()
        {
            _repository.Setup(x => x.ListAsync()).Returns(Task.FromResult(new List<Models.TestModel> { new Models.TestModel(), new Models.TestModel(), new Models.TestModel() }.AsEnumerable()));

            var models = (await _repository.Object.ListAsync()).ToList();

            _repository.Verify(x => x.ListAsync(), Times.Once());

            Assert.Equal(3, models.Count);
        }

        [Fact]
        public void FindWithQueryReturnsData()
        {
            const string query = "Property = 0";

            _repository.Setup(x => x.Where(query)).Returns(new List<Models.TestModel> { new Models.TestModel(), new Models.TestModel(), new Models.TestModel() });

            var models = _repository.Object.Where(query).ToList();

            _repository.Verify(x => x.Where(query), Times.Once());

            Assert.Equal(3, models.Count);
        }

        [Fact]
        public async Task FindAsyncWithQueryReturnsData()
        {
            const string query = "Property = 0";

            _repository.Setup(x => x.WhereAsync(query)).Returns(Task.FromResult(new List<Models.TestModel> { new Models.TestModel(), new Models.TestModel(), new Models.TestModel() }.AsEnumerable()));

            var models = (await _repository.Object.WhereAsync(query)).ToList();

            _repository.Verify(x => x.WhereAsync(query), Times.Once());

            Assert.Equal(3, models.Count);
        }
    }
}
