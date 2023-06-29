using System.Collections.Generic;

namespace laget.Db.Dapper.Models
{
    public class PaginationResult<TEntity>
    {
        public IEnumerable<TEntity> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
