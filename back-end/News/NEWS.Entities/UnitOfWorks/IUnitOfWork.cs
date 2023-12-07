using Microsoft.EntityFrameworkCore;
using NEWS.Entities.Repositories;

namespace NEWS.Entities.UnitOfWorks
{
    public interface IUnitOfWork
    {
        DbContext DbContext { get; }

        int SaveChanges();
        Task SaveChangesAsync();
        void Dispose();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task BeginTransaction();
        Task Commit();
        Task RollBack();
    }
}
