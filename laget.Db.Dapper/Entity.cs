using System;

namespace laget.Db.Dapper
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public abstract object ToObject();
    }
}
