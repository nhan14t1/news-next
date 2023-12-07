using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Entities.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        DbSet<TEntity> Entities { get; }

        DbContext DbContext { get; set; }

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        void Add(TEntity entity, bool saveChanges = true);
        void Update(TEntity entity, bool saveChanges = true);
        void Delete(TEntity entity, bool saveChanges = true);

        Task AddAsync(TEntity entity, bool saveChanges = true);
        Task UpdateAsync(TEntity entity, bool saveChanges = true);
        Task DeleteAsync(TEntity entity, bool saveChanges = true);
    }
}
