using System;

namespace laget.Db.Dapper.Tests.Models
{
    public class AccountModel : Entity
    {
        public string FirstName { get; set; } = "Jane";
        public string LastName { get; set; } = "Doe";
        public string Email { get; set; }

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