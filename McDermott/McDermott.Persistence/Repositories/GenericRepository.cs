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
    public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public void Attach(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
        }

        public void SetPropertyModified(T entity, string propertyName)
        {
            _dbContext.Entry(entity).Property(propertyName).IsModified = true;
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                // Ambil properti dengan SetToNullAttribute dari cache
                var propertiesToSetToNull = PropertyCacheHelper.GetSetToNullProperties<T>();

                // Gunakan loop foreach untuk mengatur nilai properti menjadi null
                foreach (var property in propertiesToSetToNull)
                {
                    property.SetValue(entity, null);
                }

                await _dbContext.Set<T>().AddAsync(entity);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Yg Lama
        //public async Task<T> AddAsync(T entity)
        //{
        //    try
        //    {
        //        foreach (PropertyInfo property in typeof(T).GetProperties())
        //        {
        //            var nullAttribute = property.GetCustomAttribute<SetToNullAttribute>();
        //            if (nullAttribute != null)
        //            {
        //                property.SetValue(entity, null);
        //            }
        //            else
        //            {
        //                // Atur nilai properti lainnya sesuai kebutuhan
        //            }
        //        }

        //        await _dbContext.Set<T>().AddAsync(entity);
        //        return entity;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<List<T>> AddAsync(List<T> entities)
        {
            try
            {
                var propertiesToSetToNull = PropertyCacheHelper.GetSetToNullProperties<T>();

                foreach (var entity in entities)
                {
                    // Gunakan LINQ untuk mengatur nilai properti menjadi null
                    propertiesToSetToNull.ForEach(property => property.SetValue(entity, null));
                }

                await _dbContext.Set<T>().AddRangeAsync(entities);

                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // YG Lama
        //public async Task<List<T>> AddAsync(List<T> entities)
        //{
        //    try
        //    {
        //        foreach (var entity in entities)
        //        {
        //            foreach (PropertyInfo property in typeof(T).GetProperties())
        //            {
        //                var nullAttribute = property.GetCustomAttribute<SetToNullAttribute>();
        //                if (nullAttribute != null)
        //                {
        //                    property.SetValue(entity, null);
        //                }
        //                else
        //                {
        //                    // Atur nilai properti lainnya sesuai kebutuhan
        //                }
        //            }
        //        }

        //        await _dbContext.Set<T>().AddRangeAsync(entities);

        //        return entities;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                T exist = await _dbContext.Set<T>().FindAsync(entity.Id)!;

                if (exist is not null)
                {
                    // Melampirkan entitas yang sudah ada ke DbContext
                    _dbContext.Attach(exist);

                    // Menandai entitas sebagai dimodifikasi
                    _dbContext.Entry(exist).State = EntityState.Modified;

                    entity.CreatedBy = exist.CreatedBy;
                    entity.CreatedDate = exist.CreatedDate;

                    // Ambil properti dengan SetToNullAttribute dari cache
                    var propertiesToSetToNull = PropertyCacheHelper.GetSetToNullProperties<T>();

                    foreach (var property in propertiesToSetToNull)
                    {
                        property.SetValue(entity, null);
                    }

                    exist = entity.Adapt(exist);
                }

                return exist!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Yg Lama
        //public async Task<T> UpdateAsync(T entity)
        //{
        //    try
        //    {
        //        T exist = await _dbContext.Set<T>().FindAsync(entity.Id)!;

        //        if (exist is not null)
        //        {
        //            // Melampirkan entitas yang sudah ada ke DbContext
        //            _dbContext.Attach(exist);

        //            // Menandai entitas sebagai dimodifikasi
        //            _dbContext.Entry(exist).State = EntityState.Modified;

        //            entity.CreatedBy = exist.CreatedBy;
        //            entity.CreatedDate = exist.CreatedDate;

        //            foreach (PropertyInfo property in typeof(T).GetProperties())
        //            {
        //                var nullAttribute = property.GetCustomAttribute<SetToNullAttribute>();
        //                if (nullAttribute != null)
        //                {
        //                    property.SetValue(entity, null);
        //                }
        //                else
        //                {
        //                    // Atur nilai properti lainnya sesuai kebutuhan
        //                }
        //            }

        //            exist = entity.Adapt(exist);
        //        }

        //        return exist!;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<List<T>> UpdateAsync(List<T> entities)
        {
            try
            {
                var propertiesToSetToNull = PropertyCacheHelper.GetSetToNullProperties<T>();

                foreach (var entity in entities)
                {
                    T exist = await _dbContext.Set<T>().FindAsync(entity.Id);

                    if (exist is not null)
                    {
                        // Melampirkan entitas yang sudah ada ke DbContext
                        _dbContext.Attach(exist);

                        // Menandai entitas sebagai dimodifikasi
                        _dbContext.Entry(exist).State = EntityState.Modified;

                        entity.CreatedBy = exist.CreatedBy;
                        entity.CreatedDate = exist.CreatedDate;

                        // Gunakan LINQ untuk mengatur nilai properti menjadi null
                        propertiesToSetToNull.ForEach(property => property.SetValue(entity, null));

                        exist = entity.Adapt(exist);
                    }
                }

                return entities;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Yg Lama
        //public async Task<List<T>> UpdateAsync(List<T> entities)
        //{
        //    try
        //    {
        //        foreach (var entity in entities)
        //        {
        //            T exist = await _dbContext.Set<T>().FindAsync(entity.Id)!;

        //            if (exist is not null)
        //            {
        //                // Melampirkan entitas yang sudah ada ke DbContext
        //                _dbContext.Attach(exist);

        //                // Menandai entitas sebagai dimodifikasi
        //                _dbContext.Entry(exist).State = EntityState.Modified;

        //                entity.CreatedBy = exist.CreatedBy;
        //                entity.CreatedDate = exist.CreatedDate;

        //                foreach (PropertyInfo property in typeof(T).GetProperties())
        //                {
        //                    var nullAttribute = property.GetCustomAttribute<SetToNullAttribute>();
        //                    if (nullAttribute != null)
        //                    {
        //                        property.SetValue(entity, null);
        //                    }
        //                    else
        //                    {
        //                        // Atur nilai properti lainnya sesuai kebutuhan
        //                    }
        //                }

        //                exist = entity.Adapt(exist);
        //            }
        //        }

        //        return entities!;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task DeleteAsync(long id)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);

                _dbContext.Set<T>().Remove(entity!);
            }
            catch (Exception ex)
            {
                LogError(nameof(DeleteAsync), ex.Message, ex);
            }
        }

        public async Task DeleteAsync(bool deleteAll = false)
        {
            try
            {
                if (deleteAll)
                {
                    var entity = await _dbContext.Set<T>().ToListAsync();

                    if (entity is not null && entity.Count > 0)
                        _dbContext.Set<T>().RemoveRange(entity);
                }
            }
            catch (InvalidOperationException ex)
            {
                LogError(nameof(DeleteAsync), ex.Message, ex);
            }
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = await _dbContext.Set<T>()
                                        .Where(predicate)
                                        .ToListAsync();
                if (entity is not null)
                    _dbContext.Set<T>().RemoveRange(entity);
            }
            catch (InvalidOperationException ex)
            {
                LogError(nameof(DeleteAsync), ex.Message, ex);
            }
        }

        public async Task DeleteAsync(List<long> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var entity = await _dbContext.Set<T>().FindAsync(id);

                    if (entity is not null)
                        _dbContext.Set<T>().Remove(entity!);
                }
            }
            catch (InvalidOperationException ex)
            {
                LogError(nameof(DeleteAsync), ex.Message, ex);
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

        public void LogError(dynamic method, dynamic result, Exception? ex = null)
        {
            //Log.Error(ex, method + " {Handler} => {@Result}", method, ex);

            Log.Error(method + "ERROR => {@result}", ex);
            //Log.Error("Ngeteh Njay", "Unhandled Exception occurred Halo");
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
                LogError(nameof(GetAsync), ex.Message, ex);
                throw;
            }
        }

        public async Task<T> GetByIdAsync(long id)
        {
            try
            {
                var result = await _dbContext.Set<T>().FindAsync(id);

                LogInformation(nameof(GetAsync), result);

                return result;
            }
            catch (Exception ex)
            {
                LogError(nameof(GetAsync), ex.Message, ex);
                return default(T);
            }
        }

        //public async Task UpdateAsync(List<T> entity)
        //{
        //    foreach (var item in entity)
        //    {
        //        T exist = _dbContext.Set<T>().Find(item.Id);
        //        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        //    }
        //}

        public async Task<long> GetCountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.AsNoTracking().CountAsync(cancellationToken);
        }

        public IQueryable<T> GetAllQuerylable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
    }
}

public static class PropertyCacheHelper
{
    private static readonly Dictionary<Type, List<PropertyInfo>> SetToNullPropertiesCache = new();

    public static List<PropertyInfo> GetSetToNullProperties<T>()
    {
        var type = typeof(T);
        if (!SetToNullPropertiesCache.TryGetValue(type, out var properties))
        {
            properties = type.GetProperties()
                             .Where(p => p.GetCustomAttribute<SetToNullAttribute>() != null)
                             .ToList();
            SetToNullPropertiesCache[type] = properties;
        }
        return properties;
    }
}