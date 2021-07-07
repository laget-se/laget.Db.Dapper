using System;
using laget.Db.Dapper.Extensions;

namespace laget.Db.Dapper.Tests.Models
{
    [DapperTable("Attributes")]
    public class AttributeTestModel : Entity
    {
        [DapperColumn("intId")]
        public override int Id { get; set; }
        public string Column1 { get; set; }
        public bool Column2 { get; set; }
        public int Column3 { get; set; }
        public string Column4 { get; set; }
        [DapperColumn("Column6")]
        public int? Column5 { get; set; }

        public AttributeTestModel()
        {
            Id = int.MaxValue;
            CreatedAt = DateTime.Now.AddMonths(-1);
            UpdatedAt = DateTime.Now;
        }

        public override object ToObject() => new
        {
            Column1,
            Column2,
            Column3,
            Column4,
            Column5
        };
    }
}
