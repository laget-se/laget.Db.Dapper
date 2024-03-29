﻿using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using System;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class RepositorySqlTests : IClassFixture<RepositoryFixture<TestModel>>
    {
        private readonly TestRepository<TestModel> _repository;

        public RepositorySqlTests(RepositoryFixture<TestModel> fixture)
        {
            _repository = fixture.Repository;
        }

        [Fact]
        public void GetInsertQueryGeneratesSql()
        {
            var model = new TestModel
            {
                Column1 = "foo",
                Column2 = true,
                Column3 = 1337,
                Column4 = null,
                Column5 = null
            };

            var query = _repository.ExposedGetInsertQuery(model);
            dynamic parameters = query.parameters;

            Assert.Equal("INSERT INTO [TestModel] ([Column1],[IsActivated],[Column3],[Column4],[Column5]) OUTPUT INSERTED.[Id] VALUES (@Column1,@Column2,@Column3,@Column4,@Column5)", query.sql);
            Assert.Equal(model.Column1, parameters.Column1);
            Assert.Equal(model.Column2, parameters.Column2);
            Assert.Equal(model.Column3, parameters.Column3);
            Assert.Equal(model.Column4, parameters.Column4);
            Assert.Equal(model.Column5, parameters.Column5);

            Assert.Equal(int.MaxValue, model.Id);
            Assert.Equal(DateTime.Now.AddMonths(-1).ToShortDateString(), model.CreatedAt.ToShortDateString());
            Assert.Equal(DateTime.Now.ToShortDateString(), model.UpdatedAt.ToShortDateString());
        }

        [Fact]
        public void GetUpdateQueryGeneratesSql()
        {
            var model = new TestModel
            {
                Id = 666,
                Column1 = "foo",
                Column2 = true,
                Column3 = 1337,
                Column4 = null,
                Column5 = null
            };

            var query = _repository.ExposedGetUpdateQuery(model);
            dynamic parameters = query.parameters;

            Assert.Equal("UPDATE [TestModel] SET [Column1] = @Column1, [IsActivated] = @Column2, [Column3] = @Column3, [Column4] = @Column4, [Column5] = @Column5 WHERE [Id] = @Id", query.sql);
            Assert.Equal(model.Column1, parameters.Column1);
            Assert.Equal(model.Column2, parameters.Column2);
            Assert.Equal(model.Column3, parameters.Column3);
            Assert.Equal(model.Column4, parameters.Column4);
            Assert.Equal(model.Column5, parameters.Column5);
        }

        [Fact]
        public void GetDeleteQueryGeneratesSql()
        {
            var model = new TestModel
            {
                Id = 666
            };

            var query = _repository.ExposedGetDeleteQuery(model);
            dynamic parameters = query.parameters;

            Assert.Equal("DELETE FROM [TestModel] WHERE [Id] = @Id", query.sql);
            Assert.Equal(666, model.Id);
        }

        [Fact]
        public void GetWhereQueryGeneratesSql()
        {
            var query = _repository.ExposedGetWhereQuery("Property = 0 AND Status = 1");

            Assert.Equal("SELECT * FROM [TestModel] WHERE Property = 0 AND Status = 1", query);
        }
    }
}