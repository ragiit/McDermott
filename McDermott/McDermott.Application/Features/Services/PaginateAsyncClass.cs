using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Services
{
    public static class PaginateAsyncClass
    {
        public static async Task<(int totalCount, IQueryable<T> paged, int totalPages)> PaginateAsync<T>(
             int pageSize,
             int pageIndex,
             IQueryable<T> query,
             IOrderedQueryable<T>? pagedResult,
             CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            pageSize = pageSize == 0 ? totalCount : pageSize;

            var skip = (pageIndex) * pageSize;

            var paged = pagedResult == null ?
                query.Skip(skip).Take(pageSize)
                :
                pagedResult.Skip(skip).Take(pageSize);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return (totalCount, paged, totalPages);
        }

        public static async Task<(int totalCount, List<T> pagedItems, int totalPages)> PaginateAndSortAsync<T>(
                   IQueryable<T> query,
                   int pageSize,
                   int pageIndex,
                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                   CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            pageSize = pageSize == 0 ? totalCount : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagedItems = await orderBy(query)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (totalCount, pagedItems, totalPages);
        }

        // Paginate and sort helper method
        public static async Task<(int totalCount, List<T> pagedItems, int totalPages)> PaginateAndSortAsync<T>(
            IQueryable<T> query,
            int pageSize,
            int pageIndex,
            CancellationToken cancellationToken)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            pageSize = pageSize == 0 ? totalCount : pageSize;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagedItems = await query
                .Skip((pageIndex) * pageSize) // Adjust to use (pageIndex - 1) for correct pagination
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (totalCount, pagedItems, totalPages);
        }
    }
}