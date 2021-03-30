
using System.Threading.Tasks;
using laget.Db.Dapper.Exceptions;

namespace laget.Db.Dapper
{
    public class ReadOnlyRepository<TEntity> : Repository<TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        public ReadOnlyRepository(IDapperDefaultProvider provider)
            : base(provider)
        {
        }

        public override TEntity Insert(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override TEntity Update(TEntity entity)
        {
            throw new ReadOnlyException(entity.GetType());
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
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
