using System;

namespace laget.Db.Dapper.Exceptions
{
    public class ReadOnlyRepositoryException : Exception
    {
        public ReadOnlyRepositoryException(string message)
            : base(message)
        {
        }

        public ReadOnlyRepositoryException(object type)
            : base($"We're not allowing writes to the read only repository: {nameof(type.GetType)}")
        {
        }
    }
}
