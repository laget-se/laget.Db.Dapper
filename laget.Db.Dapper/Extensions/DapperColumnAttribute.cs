using System;

namespace laget.Db.Dapper.Extensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
    public class DapperColumnAttribute : Attribute
    {
        public virtual string ColumnName { get; }

        public DapperColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
