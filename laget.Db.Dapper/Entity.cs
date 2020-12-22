using System;
using laget.Db.Dapper.Exceptions;

namespace laget.Db.Dapper
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public abstract object ToObject();
    }

    public class ReadOnlyEntity : Entity
    {
        public override object ToObject() => throw new ReadOnlyException(GetType());
    }
}
