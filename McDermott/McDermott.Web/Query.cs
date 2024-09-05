using GreenDonut;
using MailKit.Search;
using McDermott.Persistence.Context;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;

namespace McDermott.Web
{
    public class Query
    {
        public async Task<ItemConnection<T>> GetPagedData<T>(
            IQueryable<T> query,
            int pageIndex = 1,
            int pageSize = 10) where T : class
        {
            var skip = (pageIndex - 1) * pageSize;

            // Get the total count of filtered records
            var totalCount = await query.CountAsync();

            // Apply pagination
            var paged = query
                        //.OrderBy(v => EF.Property<object>(v, "Name"))
                        .Skip(skip)
                        .Take(pageSize);

            // Calculate total page count based on pageSize
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new ItemConnection<T>
            {
                ActivePageIndexCount = totalPages,
                Items = paged
            };
        }

        public Task<ItemConnection<City>> Cities(
          [Service] ApplicationDbContext context,
          int pageIndex = 1,
          int pageSize = 10,
          string searchTerm = "")
        {
            var query = context.Cities
                .AsNoTracking()
                .Where(v => string.IsNullOrEmpty(searchTerm) ||
                    v.Name.Contains(searchTerm) ||
                    v.Province.Name.Contains(searchTerm));

            var pagedResult = query
                .OrderBy(x => x.Name)
                .Include(v => v.Province);

            return GetPagedData(pagedResult, pageIndex, pageSize);
        }

        public Task<ItemConnection<District>> Districts(
            [Service] ApplicationDbContext context,
            int pageIndex = 1,
            int pageSize = 10,
            string searchTerm = "")
        {
            var query = context.Districts
                .AsNoTracking()
                .Where(v => string.IsNullOrEmpty(searchTerm) ||
                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.City.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Province.Name, $"%{searchTerm}%"));

            var pagedResult = query
                .OrderBy(x => x.Name)
                .Include(v => v.City)
                .Include(v => v.Province);

            return GetPagedData(pagedResult, pageIndex, pageSize);
        }

        public Task<ItemConnection<Province>> Provinces(
            [Service] ApplicationDbContext context,
            int pageIndex = 1,
            int pageSize = 10,
            string searchTerm = "")
        {
            var query = context.Provinces
                .AsNoTracking()
                .Where(v => string.IsNullOrEmpty(searchTerm) ||
                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Country.Name, $"%{searchTerm}%"));

            var pagedResult = query
                .OrderBy(x => x.Name)
                .Include(v => v.Country);

            return GetPagedData(pagedResult, pageIndex, pageSize);
        }

        public Task<ItemConnection<Country>> Countries(
            [Service] ApplicationDbContext context,
            int pageIndex = 1,
            int pageSize = 10,
            string searchTerm = "")
        {
            var query = context.Countries
                .AsNoTracking()
                .Where(v => string.IsNullOrEmpty(searchTerm) ||
                    EF.Functions.Like(v.Name, $"%{searchTerm}%"));

            return GetPagedData(query, pageIndex, pageSize);
        }

        public Task<ItemConnection<Village>> Villages(
            [Service] ApplicationDbContext context,
            int pageIndex = 1,
            int pageSize = 10,
            string searchTerm = "")
        {
            var query = context.Villages
                        .AsNoTracking()
                        .Where(v => string.IsNullOrEmpty(searchTerm) ||
                                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                                    EF.Functions.Like(v.City.Name, $"%{searchTerm}%") ||
                                    EF.Functions.Like(v.Province.Name, $"%{searchTerm}%"));

            var pagedResult = query
                .OrderBy(x => x.Name)
                .Include(v => v.City)
                .Include(v => v.Province)
                .Include(v => v.District);

            return GetPagedData(pagedResult, pageIndex, pageSize);
        }
    }

    public class GraphQLResponse<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public class PaginationResult<T>
    {
        [JsonPropertyName("activePageIndexCount")]
        public int ActivePageIndexCount { get; set; }

        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = [];
    }

    public class ItemConnection<T>
    {
        public int ActivePageIndexCount { get; set; }
        public IQueryable<T> Items { get; set; }
    }
}