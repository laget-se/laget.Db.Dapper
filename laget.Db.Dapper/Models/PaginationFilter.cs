﻿using laget.Db.Dapper.Enums;

namespace laget.Db.Dapper.Models
{
    public class PaginationFilter
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 500;

        public string OrderByColumn { get; set; }
        public Order Order { get; set; }

        public int Offset => (CurrentPage - 1) * PageSize;
    }
}
