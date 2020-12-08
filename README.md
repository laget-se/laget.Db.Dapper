# laget.Db.Dapper
A generic implementation of Dapper, a high performance Micro-ORM supporting SQL Server, MySQL, Sqlite, SqlCE, Firebird etc...

## Usage
```c#
public interface IUserRepository : IRepository<Models.User>
{
}

public class UserRepository : CacheableRepository<Models.User>, IUserRepository
{
    public UserRepository(string connectionString)
        : base(connectionString, new Cache(nameof(UserRepository)))
    {
    }
}
```
