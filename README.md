# laget.Db.Dapper
A repository pattern implementation for Dapper, a high performance Micro-ORM supporting SQL Server, MySQL, Sqlite, SqlCE, Firebird etc...

![Nuget](https://img.shields.io/nuget/v/laget.Db.Dapper)
![Nuget](https://img.shields.io/nuget/dt/laget.Db.Dapper)

## Configuration
> This example is shown using Autofac since this is the go-to IoC for us.
```c#
public class DatabaseModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new DapperDefaultProvider(c.Resolve<IConfiguration>().GetConnectionString("SqlConnectionString"))).As<IDapperDefaultProvider>().SingleInstance();
    }
}
```
```c#
public class DatabaseModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new DapperDefaultProvider(c.Resolve<IConfiguration>().GetConnectionString("SqlConnectionString"), new MemoryCacheOptions
            {
                CompactionPercentage = 0.25,
                ExpirationScanFrequency = TimeSpan.FromMinutes(5),
                SizeLimit = 1024
            })).As<IDapperDefaultProvider>().SingleInstance();
    }
}
```

## Usage
### Built-in methods
```c#
public interface IRepository<TEntity>
{
    IEnumerable<TEntity> Find();
    Task<IEnumerable<TEntity>> FindAsync();
    TEntity Get(int id);
    Task<TEntity> GetAsync(int id);

    int Insert(TEntity entity);
    Task<int> InsertAsync(TEntity entity);

    void Update(TEntity entity);
    Task UpdateAsync(TEntity entity);

    void Delete(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
```

### Generic
```c#
public interface IUserRepository : IRepository<Models.User>
{
}

public class UserRepository : Repository<Models.User>, IUserRepository
{
    public UserRepository(IDapperDefaultProvider provider)
        : base(provider)
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
    public UserRepository(IDapperDefaultProvider provider)
        : base(provider)
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
