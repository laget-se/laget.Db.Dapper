using System.ComponentModel;

namespace laget.Db.Dapper.Enums
{
    public enum Order
    {
        [Description("ASC")]
        Ascending,
        [Description("DESC")]
        Descending
    }
}
