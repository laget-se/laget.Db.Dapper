﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using laget.Db.Dapper.Tests.Fixtures;
using Moq;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class RepositoryMethodTests
    {
        readonly Mock<TestRepository<Models.TestModel>> _repository;

        public RepositoryMethodTests()
        {
            _repository = new Mock<TestRepository<Models.TestModel>> { CallBase = true };
        }

        [Fact]
        public void FindReturnsData()
        {
            _repository.Setup(x => x.Find()).Returns(new List<Models.TestModel> { new Models.TestModel(), new Models.TestModel(), new Models.TestModel() });

            var models = _repository.Object.Find().ToList();

            _repository.Verify(x => x.Find(), Times.Once());

            Assert.Equal(3, models.Count);
        }

        [Fact]
        public async Task FindAsyncReturnsData()
        {
            _repository.Setup(x => x.FindAsync()).Returns(Task.FromResult(new List<Models.TestModel> { new Models.TestModel(), new Models.TestModel(), new Models.TestModel() }.AsEnumerable()));

            var models = (await _repository.Object.FindAsync()).ToList();

            _repository.Verify(x => x.FindAsync(), Times.Once());

            Assert.Equal(3, models.Count);
        }
    }
}
