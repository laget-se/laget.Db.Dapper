namespace laget.Db.Dapper.Models
{
    public class PaginationFilter
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 500;

        public int Offset => (CurrentPage - 1) * PageSize;

    }
}
