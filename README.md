# laget.Db.Dapper
A generic implementation of Dapper, a high performance Micro-ORM supporting SQL Server, MySQL, Sqlite, SqlCE, Firebird etc...

## Usage
```c#
public interface IUserRepository : IRepository<Models.User>
{
}

public class UserRepository : Repository<Models.User>, IUserRepository
{
    public UserRepository(string connectionString)
        : base(connectionString)
    {
    }
}
```
