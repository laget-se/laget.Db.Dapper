using Dapper;
using laget.Db.Dapper.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace laget.Db.Dapper
{
    public interface IPageableRepository<TEntity> : IRepository<TEntity>
    {
        PaginationResult<TEntity> List(PaginationFilter filter);
        Task<PaginationResult<TEntity>> ListAsync(PaginationFilter filter);
        PaginationResult<TEntity> List(PaginationFilter filter, string where);
        Task<PaginationResult<TEntity>> ListAsync(PaginationFilter filter, string where);
    }

    public class PageableRepository<TEntity> : Repository<TEntity>, IIPageableRepository<TEntity> where TEntity : Entity
    {
        public PageableRepository(IDapperDefaultProvider provider)
            : base(provider)
        {
        }

        public PaginationResult<TEntity> List(PaginationFilter filter)
        {
            var result = new PaginationResult<TEntity>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var sql = $@"
                    SELECT t.*
                    FROM [{TableName}] t
                    ORDER BY Id
                    OFFSET @offset ROWS
                    FETCH NEXT @size ROWS ONLY;
                    SELECT COUNT(*)
                    FROM [{TableName}];";

                var parameters = new
                {
                    offset = filter.Offset,
                    size = filter.PageSize
                };

                using (var gr = connection.QueryMultiple(sql, parameters))
                {
                    result.Items = gr.Read<TEntity>();
                    result.TotalCount = gr.ReadFirst<int>();
                }
            }

            return result;
        }

        public async Task<PaginationResult<TEntity>> ListAsync(PaginationFilter filter)
        {
            var result = new PaginationResult<TEntity>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var sql = $@"
                    SELECT t.*
                    FROM [{TableName}] t
                    ORDER BY Id
                    OFFSET @offset ROWS
                    FETCH NEXT @size ROWS ONLY;
                    SELECT COUNT(*)
                    FROM [{TableName}];";

                var parameters = new
                {
                    offset = filter.Offset,
                    size = filter.PageSize
                };

                using (var gr = await connection.QueryMultipleAsync(sql, parameters))
                {
                    result.Items = gr.Read<TEntity>();
                    result.TotalCount = gr.ReadFirst<int>();
                }
            }

            return result;
        }
        public PaginationResult<TEntity> List(PaginationFilter filter, string where)
        {
            var result = new PaginationResult<TEntity>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var sql = $@"
                    SELECT t.*
                    FROM [{TableName}] t
                    WHERE {where}
                    ORDER BY Id
                    OFFSET @offset ROWS
                    FETCH NEXT @size ROWS ONLY;
                    SELECT COUNT(*)
                    FROM [{TableName}]
                    WHERE {where};";

                var parameters = new
                {
                    offset = filter.Offset,
                    size = filter.PageSize
                };

                using (var gr = connection.QueryMultiple(sql, parameters))
                {
                    result.Items = gr.Read<TEntity>();
                    result.TotalCount = gr.ReadFirst<int>();
                }
            }

            return result;
        }


        public async Task<PaginationResult<TEntity>> ListAsync(PaginationFilter filter, string where)
        {
            var result = new PaginationResult<TEntity>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                var sql = $@"
                    SELECT t.*
                    FROM [{TableName}] t
                    WHERE {where}
                    ORDER BY Id
                    OFFSET @offset ROWS
                    FETCH NEXT @size ROWS ONLY;
                    SELECT COUNT(*)
                    FROM [{TableName}]
                    WHERE {where};";

                var parameters = new
                {
                    offset = filter.Offset,
                    size = filter.PageSize
                };

                using (var gr = await connection.QueryMultipleAsync(sql, parameters))
                {
                    result.Items = gr.Read<TEntity>();
                    result.TotalCount = gr.ReadFirst<int>();
                }
            }

            return result;
        }
    }
}
