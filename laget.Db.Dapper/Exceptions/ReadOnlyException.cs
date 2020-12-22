using System;
using System.Reflection;

namespace laget.Db.Dapper.Exceptions
{
    public class ReadOnlyException : Exception
    {
        public ReadOnlyException()
            : base("We're not allowing writes to a read-only repository")
        {
        }

        public ReadOnlyException(string message)
            : base(message)
        {
        }

        public ReadOnlyException(MemberInfo type)
            : base($"We're not allowing writes to a read-only repository: {type.Name}")
        {
        }
    }
}
