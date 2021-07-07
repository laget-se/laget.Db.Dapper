using System;
using laget.Db.Dapper.Extensions;

namespace laget.Db.Dapper.Tests.Models
{
    public class AccountModel : Entity
    {
        [DapperColumn("intAccountId")]
        public override int Id { get; set; } = 1;
        public string FirstName { get; set; } = "Jane";
        public string LastName { get; set; } = "Doe";
        public string Email { get; set; }
        [DapperColumn("intAccountId")]
        public string SocialSecurityNumber = "1986860320-5009";

        public AccountModel()
        {
            Email = $"{FirstName.ToLower()}.{LastName.ToLower()}@laget.se";
            CreatedAt = DateTime.Now.AddMonths(-1);
            UpdatedAt = DateTime.Now;
        }

        public override object ToObject() => new
        {
            FirstName,
            LastName,
            Email
        };
    }
}