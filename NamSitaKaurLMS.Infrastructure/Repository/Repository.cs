using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Core.Abstract;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;

namespace NamSitaKaurLMS.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly NamSitaKaurLMSContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(NamSitaKaurLMSContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} bulunamadı. Id: {id}");

            _dbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (!tracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} bulunamadı. Id: {id}");

            return entity;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
