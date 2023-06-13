using laget.Db.Dapper.Extensions;

namespace laget.Db.Dapper.Tests.Models
{
    [DapperTable("Table")]
    public class EmptyModel : Entity
    {
        public override object ToObject() => new
        {
        };
    }
}
