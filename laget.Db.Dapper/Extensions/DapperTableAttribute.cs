﻿using System;

namespace laget.Db.Dapper.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DapperTableAttribute : Attribute
    {
        public virtual string TableName { get; }

        public DapperTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
