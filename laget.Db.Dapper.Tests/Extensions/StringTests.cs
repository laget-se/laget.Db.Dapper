using laget.Db.Dapper.Extensions;
using Xunit;

namespace laget.Db.Dapper.Tests.Extensions
{
    public class StringTests
    {
        [Fact]
        public void ShouldValidateQuery()
        {
            const string query = @"
                SELECT * FROM Person WHERE Gender = 1";

            var valid = query.IsValid(out var errors);

            Assert.True(valid);
            Assert.Empty(errors);
        }

        [Fact]
        public void ShouldReturnErrorsForInvalidQuery()
        {
            const string query = @"
                SELECT * FROM Person WHERE )%/)7%)/# = 1";

            var valid = query.IsValid(out var errors);

            Assert.False(valid);
            Assert.NotEmpty(errors);
        }
    }
}
