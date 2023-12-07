using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NEWS.Entities.DataContext;
using NEWS.Entities.Repositories;
using NEWS.Entities.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext DbContext { get; private set; }
        private readonly Dictionary<string, object> _repositories;
        private IDbContextTransaction _dbTransaction;
        private IsolationLevel? _isolationLevel;
        private bool _dispose;

        public UnitOfWork(DbFactory dbFactory)
        {
            DbContext = dbFactory.DbContext;
            _repositories = new Dictionary<string, dynamic>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_dispose)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            _dispose = true;
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            //var entityType = typeof(TEntity).Name.ToUpper();
            //if (_repositories.ContainsKey(entityType))
            //{
            //    return (IRepository<TEntity>)_repositories[entityType];
            //}

            //_repositories.Add(entityType, new Repository<TEntity>(DbContext));

            //return (IRepository<TEntity>)_repositories[entityType];

            var type = typeof(TEntity);
            var typeName = type.Name;

            lock (_repositories)
            {
                if (_repositories.ContainsKey(typeName))
                {
                    return (IRepository<TEntity>)_repositories[typeName];
                }

                var repository = new Repository<TEntity>(DbContext);

                _repositories.Add(typeName, repository);
                return repository;
            }
        }

        #region Transaction

        public async Task BeginTransaction()
        {
            //if (_dbTransaction == null)
            //{
            //    if (!DbContext.Database.CanConnect())
            //    {
            //        DbContext.Database.OpenConnection();
            //    }

            //    _dbTransaction = DbContext.Database.BeginTransaction();
            //}

            await StartNewTransactionIfNeeded();
        }

        public async Task Commit()
        {
            //DbContext.SaveChanges();
            //if (_dbTransaction != null)
            //{
            //    _dbTransaction.Commit();
            //    _dbTransaction.Dispose();
            //    _dbTransaction = null;
            //}

            //return true;

            await DbContext.SaveChangesAsync();

            if (_dbTransaction == null) return;
            await _dbTransaction.CommitAsync();

            await _dbTransaction.DisposeAsync();
            _dbTransaction = null;
        }

        public async Task RollBack()
        {
            if (_dbTransaction == null) return;

            await _dbTransaction.RollbackAsync();

            await _dbTransaction.DisposeAsync();
            _dbTransaction = null;
        }

        private async Task StartNewTransactionIfNeeded()
        {
            if (_dbTransaction == null)
            {
                _dbTransaction = _isolationLevel.HasValue ?
                    await DbContext.Database.BeginTransactionAsync(_isolationLevel.GetValueOrDefault()) : await DbContext.Database.BeginTransactionAsync();
            }
        }

        #endregion
    }
}
