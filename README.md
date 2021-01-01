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

### Caching
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
    
    public override Models.User Get(int id)
    {
        var cacheKey = "_Id_" + id;
        var item = CacheGet<Models.User>(cacheKey);

        if (item != null)
            return item;

        using (var connection = new SqlConnection(ConnectionString))
        {
            var sql = $@"
                SELECT * FROM [{TableName}]
                WHERE Id = @id";

            var parameters = new
            {
                id
            };

            var result = connection.QueryFirstOrDefault<Models.User>(sql, parameters);

            CacheAdd(cacheKey, result);

            return result;
        }
    }
}
```
