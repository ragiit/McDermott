using Mapster;
using McDermott.Application.Interfaces.Repositories;
using McDermott.Domain.Common;
using McDermott.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;
using System.Reflection;

namespace McDermott.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddAsync(List<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);

            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id)!;

            // Melampirkan entitas yang sudah ada ke DbContext
            _dbContext.Attach(exist);

            // Menandai entitas sebagai dimodifikasi
            _dbContext.Entry(exist).State = EntityState.Modified;

            entity.CreatedBy = exist.CreatedBy;
            entity.CreatedDate = exist.CreatedDate;

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                var nullAttribute = property.GetCustomAttribute<SetToNullAttribute>();
                if (nullAttribute != null)
                {
                    property.SetValue(entity, null);
                }
                else
                {
                    // Atur nilai properti lainnya sesuai kebutuhan
                }
            }

            exist = entity.Adapt(exist);

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);

                _dbContext.Set<T>().Remove(entity!);
            }
            catch (Exception ex)
            {
                LogError(nameof(DeleteAsync), ex.Message);
            }
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = await _dbContext.Set<T>()
                                        .Where(predicate)
                                        .ToListAsync();
                if (entity != null)
                {
                    _dbContext.Set<T>().RemoveRange(entity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                LogError(nameof(DeleteAsync), ex.Message);
            }
        }

        public async Task DeleteAsync(List<int> ids)
        {
            foreach (var id in ids)
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                _dbContext.Set<T>().Remove(entity!);
            }
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext
                .Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            //LogInformation(nameof(GetAllAsync), result!);

            return result;
        }

        public void LogInformation(dynamic method, dynamic result)
        {
            // Uncomment jika ingin melihat data di console
            //Log.Information(method + " => {@result}", result);
        }

        public void LogError(dynamic method, dynamic result)
        {
            Log.Error(method + "ERROR => {@result}", result);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate is null)
            {
                return await _dbContext
                    .Set<T>()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            else
            {
                return await _dbContext
                    .Set<T>()
                    .AsNoTracking()
                    .Where(predicate)
                    .ToListAsync(cancellationToken);
            }
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IQueryable<T>>? includes = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Set<T>().AsQueryable();

                if (includes is not null)
                {
                    query = includes(query);
                }

                if (predicate is not null)
                {
                    query = query.Where(predicate);
                }

                LogInformation(nameof(GetAsync), query);

                return await query.AsNoTracking().ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                LogError(nameof(GetAsync), ex.Message);
                return [];
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var result = await _dbContext.Set<T>().FindAsync(id);

                LogInformation(nameof(GetAsync), result);

                return result;
            }
            catch (Exception ex)
            {
                LogError(nameof(GetAsync), ex.Message);
                return default(T);
            }
        }

        public async Task UpdateAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                T exist = _dbContext.Set<T>().Find(item.Id);
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            }
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.AsNoTracking().CountAsync(cancellationToken);
        }
    }
}