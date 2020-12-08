using System;

namespace laget.Db.Dapper.Tests.Models
{

    public class TestModel : Entity
    {
        public string Column1 { get; set; }
        public bool Column2 { get; set; }
        public int Column3 { get; set; }
        public string Column4 { get; set; }
        public int? Column5 { get; set; }

        public TestModel()
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
