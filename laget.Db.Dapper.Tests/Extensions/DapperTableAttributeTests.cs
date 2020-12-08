using System;
using System.Collections.Generic;
using laget.Db.Dapper.Extensions;
using Xunit;

namespace laget.Db.Dapper.Tests.Extensions
{
    public class DapperTableAttributeTests
    {
        [Fact]
        public void IsAttributeMultipleFalse()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(DapperTableAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.Equal(1, attributes.Count);

            var attribute = attributes[0];
            Assert.False(attribute.AllowMultiple);
        }

        [Fact]
        public void ShouldReturnCorrectTableName()
        {
            const string expected = "tTable";
            var actual = new TestClass().TableName;

            Assert.Equal(expected, actual);
        }


        [DapperTable("tTable")]

        public class TestClass : Entity
        {
            public string TableName
            {
                get
                {
                    var attribute = (DapperTableAttribute)Attribute.GetCustomAttribute(typeof(TestClass), typeof(DapperTableAttribute));

                    return attribute == null ? nameof(TestClass) : attribute.TableName;
                }
            }

            public override object ToObject()
            {
                return new { };
            }
        }
    }
}
