namespace laget.Db.Dapper.Tests.Models
{
    public class EmptyModel : Entity
    {
        public override object ToObject() => new
        {
        };
    }
}
