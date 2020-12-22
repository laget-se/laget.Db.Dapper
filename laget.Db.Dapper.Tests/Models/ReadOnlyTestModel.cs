using System;

namespace laget.Db.Dapper.Tests.Models
{
    public class ReadOnlyTestModel : ReadOnlyEntity
    {
        public string Column1 { get; set; }
        public bool Column2 { get; set; }
        public int Column3 { get; set; }
        public string Column4 { get; set; }
        public int? Column5 { get; set; }

        public ReadOnlyTestModel()
        {
            Id = int.MaxValue;
            CreatedAt = DateTime.Now.AddMonths(-1);
            UpdatedAt = DateTime.Now;
        }
    }
}
