using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.DataContext
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<NewsContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<NewsContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}






































