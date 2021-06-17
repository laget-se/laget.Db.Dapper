using System;
using System.Collections.Generic;

namespace laget.Db.Dapper.Exceptions
{
    public class InvalidSqlException : Exception
    {
        public IEnumerable<string> Errors { get; set; }

        public InvalidSqlException()
            : base("The sql query provided is invalid")
        {
        }

        public InvalidSqlException(IEnumerable<string> errors)
            : base("The sql query provided is invalid")
        {
            Errors = errors;
        }

        public InvalidSqlException(string message, IEnumerable<string> errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}
