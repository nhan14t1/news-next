using NEWS.Entities.Repositories;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using System.Linq.Expressions;

namespace NEWS.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<TEntity> _repository;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return _repository.GetAll(expression);
        }

        public virtual TEntity GetById(int id)
        {
            return _repository.Entities.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _repository.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _repository.Delete(entity);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await _repository.DeleteAsync(entity);
        }
    }
}
