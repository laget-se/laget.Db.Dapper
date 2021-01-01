using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using laget.Db.Dapper.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

namespace laget.Db.Dapper
{
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

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly IMemoryCache Cache;
        protected readonly string ConnectionString;

        protected string CachePrefix => GetCachePrefix();
        protected string TableName => GetTableName();

        public Repository(string connectionString)
        {
            ConnectionString = connectionString;

            Cache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(1)
            });
        }

        public Repository(string connectionString, MemoryCacheOptions cacheOptions)
        {
            ConnectionString = connectionString;

            Cache = new MemoryCache(cacheOptions);
        }

        public virtual IEnumerable<TEntity> Find()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $@"
                    SELECT * FROM [{TableName}]";

                return connection.Query<TEntity>(sql);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $@"
                    SELECT * FROM [{TableName}]";

                return await connection.QueryAsync<TEntity>(sql);
            }
        }

        public virtual TEntity Get(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $@"
                    SELECT * FROM [{TableName}]
                    WHERE Id = @id";

                var parameters = new
                {
                    id
                };

                var result = connection.QuerySingleOrDefault<TEntity>(sql, parameters);

                if (result == null)
                    throw new KeyNotFoundException($"[{TableName}] with id [{id}] could not be found.");

                return result;
            }
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $@"
                    SELECT * FROM [{TableName}]
                    WHERE Id = @id";

                var parameters = new
                {
                    id
                };

                var result = await connection.QuerySingleOrDefaultAsync<TEntity>(sql, parameters);

                if (result == null)
                    throw new KeyNotFoundException($"[{TableName}] with id [{id}] could not be found.");

                return result;
            }
        }

        public virtual int Insert(TEntity entity)
        {
            var (sql, parameters) = GetInsertQuery(entity);

            using (var connection = new SqlConnection(ConnectionString))
            {
                return (int)connection.ExecuteScalar(sql, parameters);
            }
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            var (sql, parameters) = GetInsertQuery(entity);

            using (var connection = new SqlConnection(ConnectionString))
            {
                return (int)await connection.ExecuteScalarAsync(sql, parameters);
            }
        }

        public virtual void Update(TEntity entity)
        {
            var (sql, parameters) = GetUpdateQuery(entity);

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.ExecuteScalar(sql, parameters);
            }
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var (sql, parameters) = GetUpdateQuery(entity);

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteScalarAsync(sql, parameters);
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var (sql, parameters) = GetDeleteQuery(entity);

                connection.Execute(sql, parameters);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var (sql, parameters) = GetDeleteQuery(entity);

                await connection.ExecuteAsync(sql, parameters);
            }
        }


        protected TZ CacheGet<TZ>(string key)
        {
            return Cache.Get<TZ>($"{CachePrefix}_{key}");
        }

        protected void CacheAdd<TZ>(string key, TZ item, MemoryCacheEntryOptions options = null)
        {
            if (options == null)
            {
                options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(15),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
            }

            Cache.Set($"{CachePrefix}_{key}", item, options);
        }

        protected (string sql, object parameters) GetInsertQuery(TEntity entity)
        {
            var obj = entity.ToObject();

            var properties = obj.GetType().GetProperties().ToList().Select(x => x.Name);
            var columns = string.Join(",", properties);
            var keys = string.Join(",", properties.Select(x => $"@{x}"));

            var sql = $"INSERT INTO [{TableName}] ({columns}) OUTPUT inserted.Id VALUES ({keys})";

            return (sql, obj);
        }

        protected (string sql, object parameters) GetUpdateQuery(TEntity entity)
        {
            var obj = entity.ToObject();

            var properties = obj.GetType().GetProperties().ToList().Select(x => x.Name).ToList();
            var sql = $"UPDATE [{TableName}] SET ";

            for (var i = 0; i < properties.Count; i++)
            {
                var value = properties[i];

                if ((i + 1) == properties.Count)
                {
                    sql += $"{value} = @{value} ";
                    break;
                }

                sql += $"{value} = @{value}, ";
            }

            sql += $"WHERE Id = {entity.Id}";

            return (sql, obj);
        }

        protected (string sql, object parameters) GetDeleteQuery(TEntity entity)
        {
            var obj = new
            {
                entity.Id
            };

            var sql = $"DELETE FROM [{TableName}] WHERE Id = @Id";

            return (sql, obj);
        }


        static string GetCachePrefix()
        {
            var attribute = (DapperTableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(DapperTableAttribute));

            return attribute == null ? typeof(TEntity).Name : attribute.CachePrefix;
        }

        static string GetTableName()
        {
            var attribute = (DapperTableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(DapperTableAttribute));

            return attribute == null ? typeof(TEntity).Name : attribute.TableName;
        }
    }
}
