using McDermott.Application.Interfaces.Repositories;
using McDermott.Domain.Common;
using McDermott.Persistence.Context;
using Serilog;
using System.Collections;

namespace McDermott.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;
        private bool disposed;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(
                  "\n\n" +
                  "==================== START SAVE ASYNC ERROR ====================" + "\n" +
                  "Message =====> " + ex.Message + "\n" +
                  "Inner Message =====> " + ex.InnerException?.Message + "\n" +
                  "Stack Trace =====> " + ex.StackTrace?.Trim() + "\n" +
                  "==================== END SAVE ASYNC ERROR ====================" + "\n"
                  );
                throw;
            }
        }

        public Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _dbContext?.Dispose();
                }

                // Dispose unmanaged resources
                _disposed = true;
            }
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposed)
        //    {
        //        if (disposing)
        //        {
        //            // Dispose managed resources
        //            _dbContext?.Dispose();
        //        }

        //        // Dispose unmanaged resources
        //        _disposed = true;
        //    }
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposed)
        //    {
        //        if (disposing)
        //        {
        //            //dispose managed resources
        //            _dbContext.Dispose();
        //        }
        //    }
        //    //dispose unmanaged resources
        //    disposed = true;
        //}
    }
}