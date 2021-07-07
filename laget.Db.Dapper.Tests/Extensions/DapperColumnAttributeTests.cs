using System;
using System.Collections.Generic;
using laget.Db.Dapper.Extensions;
using Xunit;

namespace laget.Db.Dapper.Tests.Extensions
{
    public class DapperColumnAttributeTests
    {
        [Fact]
        public void IsAttributeMultipleFalse()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(DapperColumnAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.Equal(1, attributes.Count);

            var attribute = attributes[0];
            Assert.False(attribute.AllowMultiple);
        }
    }
}
