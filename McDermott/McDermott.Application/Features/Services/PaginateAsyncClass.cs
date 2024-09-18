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
    }
}