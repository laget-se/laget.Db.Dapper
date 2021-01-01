using System;

namespace laget.Db.Dapper.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DapperTableAttribute : Attribute
    {
        public virtual string TableName { get; }
        public virtual string CachePrefix { get; }

        public DapperTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        public DapperTableAttribute(string tableName, string cachePrefix)
        {
            TableName = tableName;
            CachePrefix = cachePrefix;
        }
    }
}
