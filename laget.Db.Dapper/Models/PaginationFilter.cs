﻿namespace laget.Db.Dapper.Models
{
    public class Pagination
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 500;
    }
}
