# laget.Db.Dapper
A generic implementation of Dapper, a high performance Micro-ORM supporting SQL Server, MySQL, Sqlite, SqlCE, Firebird etc...

![Nuget](https://img.shields.io/nuget/v/laget.Db.Dapper)
![Nuget](https://img.shields.io/nuget/dt/laget.Db.Dapper)

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
