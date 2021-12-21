using System;

namespace laget.Db.Dapper
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        public abstract object ToObject();
    }
}
