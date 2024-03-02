using McDermott.Domain.Common.Interfaces;

namespace McDermott.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? includes = null, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity);

        Task<List<T>> AddAsync(List<T> entity);

        Task UpdateAsync(T entity);

        Task UpdateAsync(List<T> entity);

        Task DeleteAsync(int id);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(List<int> id);
    }
}