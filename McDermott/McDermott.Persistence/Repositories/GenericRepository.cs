using McDermott.Application.Interfaces.Repositories;
using McDermott.Domain.Common;
using McDermott.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            T exist = _dbContext.Set<T>().Find(entity.Id);

            entity.CreatedBy = exist.CreatedBy;
            entity.CreatedDate = exist.CreatedDate;

            _dbContext.Entry(exist).CurrentValues.SetValues(entity);

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);

            _dbContext.Set<T>().Remove(entity!);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
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


        public async Task DeleteAsync(List<int> ids)
        {
            foreach (var id in ids)
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                _dbContext.Set<T>().Remove(entity!);
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext
                .Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                T exist = _dbContext.Set<T>().Find(item.Id);
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            }
        }
    }
}