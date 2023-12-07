using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Entities.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
