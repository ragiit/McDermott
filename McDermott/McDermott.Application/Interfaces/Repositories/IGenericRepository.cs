using McDermott.Domain.Common.Interfaces;

namespace McDermott.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(long id);

        IQueryable<T> GetAllQuerylable();

        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? includes = null, CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity);

        Task<List<T>> AddAsync(List<T> entity);

        Task<T> UpdateAsync(T entity);

        Task<List<T>> UpdateAsync(List<T> entity);

        Task DeleteAsync(long id);

        Task DeleteAsync(bool deleteAll);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(List<long> id);
    }
}