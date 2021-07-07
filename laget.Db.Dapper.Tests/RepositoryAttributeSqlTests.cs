using System;
using laget.Db.Dapper.Tests.Fixtures;
using laget.Db.Dapper.Tests.Models;
using Xunit;

namespace laget.Db.Dapper.Tests
{
    public class RepositoryAttributeSqlTests : IClassFixture<RepositoryFixture<AttributeTestModel>>
    {
        private readonly TestRepository<AttributeTestModel> _repository;

        public RepositoryAttributeSqlTests(RepositoryFixture<AttributeTestModel> fixture)
        {
            _repository = fixture.Repository;
        }

        [Fact]
        public void GetInsertQueryGeneratesSql()
        {
            var model = new AttributeTestModel
            {
                Column1 = "foo",
                Column2 = true,
                Column3 = 1337,
                Column4 = null,
                Column5 = null
            };

            var query = _repository.ExposedGetInsertQuery(model);
            dynamic parameters = query.parameters;

            Assert.Equal("INSERT INTO [Attributes] ([Column1],[Column2],[Column3],[Column4],[Column6]) OUTPUT INSERTED.[intId] VALUES (@Column1,@Column2,@Column3,@Column4,@Column6)", query.sql);
            Assert.Equal(model.Column1, parameters.Column1);
            Assert.Equal(model.Column2, parameters.Column2);
            Assert.Equal(model.Column3, parameters.Column3);
            Assert.Equal(model.Column4, parameters.Column4);
            Assert.Equal(model.Column5, parameters.Column5);

            Assert.Equal(model.Id, int.MaxValue);
            Assert.Equal(model.CreatedAt.ToShortDateString(), DateTime.Now.AddMonths(-1).ToShortDateString());
            Assert.Equal(model.UpdatedAt.ToShortDateString(), DateTime.Now.ToShortDateString());
        }

        [Fact]
        public void GetUpdateQueryGeneratesSql()
        {
            var model = new AttributeTestModel
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

            Assert.Equal("UPDATE [Attributes] SET [Column1] = @Column1, [Column2] = @Column2, [Column3] = @Column3, [Column4] = @Column4, [Column6] = @Column6 WHERE [intId] = @Id", query.sql);
            Assert.Equal(model.Column1, parameters.Column1);
            Assert.Equal(model.Column2, parameters.Column2);
            Assert.Equal(model.Column3, parameters.Column3);
            Assert.Equal(model.Column4, parameters.Column4);
            Assert.Equal(model.Column5, parameters.Column5);
        }

        [Fact]
        public void GetDeleteQueryGeneratesSql()
        {
            var model = new AttributeTestModel
            {
                Id = 666
            };

            var query = _repository.ExposedGetDeleteQuery(model);
            dynamic parameters = query.parameters;

            Assert.Equal("DELETE FROM [Attributes] WHERE [intId] = @Id", query.sql);
            Assert.Equal(666, model.Id);
        }

        [Fact]
        public void GetWhereQueryGeneratesSql()
        {
            var query = _repository.ExposedGetWhereQuery("Property = 0 AND Status = 1");

            Assert.Equal("SELECT * FROM [Attributes] WHERE Property = 0 AND Status = 1", query);
        }
    }
}