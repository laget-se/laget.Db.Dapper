
using System.Threading.Tasks;
using laget.Db.Dapper.Exceptions;

namespace laget.Db.Dapper
{
    public class ReadOnlyRepository<TEntity> : Repository<TEntity>, IRepository<TEntity> where TEntity : ReadOnlyEntity
    {
        public ReadOnlyRepository(string connectionString)
            : base(connectionString)
        {
        }

        public override int Insert(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override Task<int> InsertAsync(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override void Update(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override Task UpdateAsync(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override void Delete(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override Task DeleteAsync(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }
    }
}
