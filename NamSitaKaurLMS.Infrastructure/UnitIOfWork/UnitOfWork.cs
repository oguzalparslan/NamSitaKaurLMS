using NamSitaKaurLMS.Core.Abstract;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;
using NamSitaKaurLMS.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly NamSitaKaurLMSContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(NamSitaKaurLMSContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : EntityBase
    {
        var type = typeof(TEntity);

        if (!_repositories.ContainsKey(type))
        {
            var repo = new Repository<TEntity>(_context);
            _repositories[type] = repo;
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}
