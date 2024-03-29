﻿using Dapper;
using laget.Db.Dapper.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace laget.Db.Dapper
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> List();
        Task<IEnumerable<TEntity>> ListAsync();

        IEnumerable<TEntity> Where(string conditions);
        Task<IEnumerable<TEntity>> WhereAsync(string conditions);

        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);
        IEnumerable<TEntity> Get(int[] ids);
        Task<IEnumerable<TEntity>> GetAsync(int[] ids);

        TEntity Insert(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        Task InsertAsync(IEnumerable<TEntity> entities);

        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        void Update(IEnumerable<TEntity> entities);
        Task UpdateAsync(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
        Task DeleteAsync(IEnumerable<TEntity> entities);
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly IMemoryCache Cache;
        protected readonly string ConnectionString;

        protected string CachePrefix => GetCachePrefix();
        protected string TableName => GetTableName();

        public Repository(IDapperDefaultProvider provider)
        {
            Cache = new MemoryCache(provider.CacheOptions);
            ConnectionString = provider.ConnectionString;

            SqlMapper.SetTypeMap(typeof(TEntity), new CustomPropertyTypeMap(typeof(TEntity), (type, columnName)
                => type.GetProperties().FirstOrDefault(prop => GetColumnName(prop) == columnName.ToLower())));
        }

        public virtual IEnumerable<TEntity> List()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $@"
                    SELECT *
                    FROM [{TableName}]";

                return connection.Query<TEntity>(sql);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> ListAsync()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = $@"
                    SELECT *
                    FROM [{TableName}]";

                return await connection.QueryAsync<TEntity>(sql);
            }
        }

        public virtual IEnumerable<TEntity> Where(string conditions)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = GetWhereQuery(conditions);

                return connection.Query<TEntity>(sql);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> WhereAsync(string conditions)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var sql = GetWhereQuery(conditions);

                return await connection.QueryAsync<TEntity>(sql);
            }
        }

        public virtual TEntity Get(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var entity = Activator.CreateInstance<TEntity>();
                var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

                var sql = $@"
                    SELECT *
                    FROM [{TableName}]
                    WHERE [{index.Key}] = @id";

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
                var entity = Activator.CreateInstance<TEntity>();
                var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

                var sql = $@"
                    SELECT *
                    FROM [{TableName}]
                    WHERE [{index.Key}] = @id";

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

        public virtual IEnumerable<TEntity> Get(int[] ids)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var entity = Activator.CreateInstance<TEntity>();
                var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

                var sql = $@"
                    SELECT *
                    FROM [{TableName}]
                    WHERE [{index.Key}] IN @ids";

                var parameters = new
                {
                    ids
                };

                var result = connection.Query<TEntity>(sql, parameters);

                return result;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(int[] ids)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var entity = Activator.CreateInstance<TEntity>();
                var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

                var sql = $@"
                    SELECT *
                    FROM [{TableName}]
                    WHERE [{index.Value}] IN @ids";

                var parameters = new
                {
                    ids
                };

                var result = await connection.QueryAsync<TEntity>(sql, parameters);

                return result;
            }
        }

        public virtual TEntity Insert(TEntity entity)
        {
            var (sql, parameters) = GetInsertQuery(entity);

            int id;

            using (var connection = new SqlConnection(ConnectionString))
            {
                id = (int)connection.ExecuteScalar(sql, parameters);
            }

            return Get(id);
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var (sql, parameters) = GetInsertQuery(entity);

            int id;

            using (var connection = new SqlConnection(ConnectionString))
            {
                id = (int)await connection.ExecuteScalarAsync(sql, parameters);
            }

            return await GetAsync(id);
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            var (sql, parameters) = GetInsertQuery(entities.First());

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(sql, entities, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            var (sql, parameters) = GetInsertQuery(entities.First());

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, entities, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }
        }

        public virtual TEntity Update(TEntity entity)
        {
            var (sql, parameters) = GetUpdateQuery(entity);

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(sql, entity, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }

            return Get(entity.Id);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var (sql, parameters) = GetUpdateQuery(entity);

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, entity, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }

            return await GetAsync(entity.Id);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            var (sql, parameters) = GetUpdateQuery(entities.First());

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(sql, entities, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            var (sql, parameters) = GetUpdateQuery(entities.First());

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, entities, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
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

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            var (sql, parameters) = GetDeleteQuery(entities.First());

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(sql, entities, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            var (sql, parameters) = GetDeleteQuery(entities.First());

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, entities, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new TransactionException($"An error occurred when executing sql transaction ({ex.Message})", ex);
                    }
                }
            }
        }


        protected (string sql, object parameters) GetInsertQuery(TEntity entity)
        {
            var obj = entity.ToObject();
            var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

            var properties = obj.GetType().GetProperties().Select(x => GetColumnName(entity, x));
            if (!properties.Any())
                return ($"INSERT INTO [{TableName}] OUTPUT INSERTED.[{index.Key}] DEFAULT VALUES", null);

            var columns = string.Join(",", properties.Select(x => $"[{x.Key}]"));
            var keys = string.Join(",", properties.Select(x => $"@{x.Value}"));

            var sql = $"INSERT INTO [{TableName}] ({columns}) OUTPUT INSERTED.[{index.Key}] VALUES ({keys})";

            return (sql, obj);
        }

        protected (string sql, object parameters) GetUpdateQuery(TEntity entity)
        {
            var obj = entity.ToObject();

            var properties = obj.GetType().GetProperties().Select(x => GetColumnName(entity, x));
            var columnKeys = string.Join(", ", properties.Select(x => $"[{x.Key}] = @{x.Value}"));
            var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

            var sql = $"UPDATE [{TableName}] SET {columnKeys} WHERE [{index.Key}] = @Id";

            return (sql, obj);
        }

        protected (string sql, object parameters) GetDeleteQuery(TEntity entity)
        {
            var obj = new
            {
                entity.Id
            };
            var index = GetColumnName(entity, entity.GetType().GetMember("Id").FirstOrDefault());

            var sql = $"DELETE FROM [{TableName}] WHERE [{index.Key}] = @Id";

            return (sql, obj);
        }

        protected string GetWhereQuery(string conditions)
        {
            var sql = $"SELECT * FROM [{TableName}] WHERE {conditions}";

            return sql;
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

        protected void CacheRemove(string key)
        {
            Cache.Remove(key);
        }

        private static string GetCachePrefix()
        {
            var attribute = (DapperTableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(DapperTableAttribute));

            return attribute == null ? typeof(TEntity).Name : attribute.CachePrefix;
        }

        private static string GetColumnName(MemberInfo member)
        {
            if (member == null) return null;

            var attribute = (DapperColumnAttribute)Attribute.GetCustomAttribute(member, typeof(DapperColumnAttribute), false);

            return (attribute?.ColumnName ?? member.Name).ToLower();
        }

        private static KeyValuePair<string, string> GetColumnName(TEntity entity, MemberInfo memberInfo)
        {
            var attribute = (DapperColumnAttribute)(entity.GetType().GetProperties()
                .FirstOrDefault(x => x.Name == memberInfo.Name))?
                .GetCustomAttribute(typeof(DapperColumnAttribute));

            return attribute != null ?
                new KeyValuePair<string, string>(attribute.ColumnName, memberInfo.Name) :
                new KeyValuePair<string, string>(memberInfo.Name, memberInfo.Name);
        }

        private static string GetTableName()
        {
            var attribute = (DapperTableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(DapperTableAttribute));

            return attribute == null ? typeof(TEntity).Name : attribute.TableName;
        }
    }
}
