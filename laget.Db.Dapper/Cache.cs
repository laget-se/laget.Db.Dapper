using System;
using System.ComponentModel.DataAnnotations;

namespace laget.Db.Dapper
{
    public class Cache
    {
        [Required]
        public string KeyPrefix { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(15);

        public Cache(string keyPrefix)
        {
            KeyPrefix = keyPrefix;
        }
    }
}
