using Google.Apis.Http;
using GreenDonut;
using MailKit.Search;
using McDermott.Persistence.Context;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Serilog;
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

        public Task<ItemConnection<Occupational>> Occupationals(
          [Service] ApplicationDbContext context,
          int pageIndex = 1,
          int pageSize = 10,
          string searchTerm = "")
        {
            var query = context.Occupationals
                .AsNoTracking()
                .Where(v => string.IsNullOrEmpty(searchTerm) ||
                    v.Name.Contains(searchTerm));

            var pagedResult = query
                .OrderBy(x => x.Name);

            return GetPagedData(pagedResult, pageIndex, pageSize);
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

            var pagedResult = query
                .OrderBy(x => x.Name);

            return GetPagedData(pagedResult, pageIndex, pageSize);
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

        public Task<ItemConnection<Menu>> Menus(
            [Service] ApplicationDbContext context,
            int pageIndex = 1,
            int pageSize = 10,
            string searchTerm = "",
            long? parentId = null)
        {
            var query = context.Menus
                .AsNoTracking()
                .Where(v =>
                    (parentId == null || v.ParentId == parentId) &&  // Fix for parentId comparison
                    (string.IsNullOrEmpty(searchTerm) ||
                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Parent.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Url, $"%{searchTerm}%")));

            var pagedResult = query
                .OrderBy(x => x.Name)
                .Include(x => x.Parent);

            return GetPagedData(pagedResult, pageIndex, pageSize);
        }

        public Task<ItemConnection<Group>> Groups(
            [Service] ApplicationDbContext context,
            int pageIndex = 1,
            int pageSize = 10,
            string searchTerm = "")
        {
            var query = context.Groups
                        .AsNoTracking()
                        .Where(v => string.IsNullOrEmpty(searchTerm) ||
                                    EF.Functions.Like(v.Name, $"%{searchTerm}%") ||
                                    EF.Functions.Like(v.Id.ToString(), $"%{searchTerm}%"));

            var pagedResult = query
                .OrderBy(x => x.Name);

            return GetPagedData(pagedResult, pageIndex, pageSize);
        }

        public Task<ItemConnection<GroupMenu>> GroupMenus(
           [Service] ApplicationDbContext context,
           int pageIndex = 1,
           int pageSize = 10,
           string searchTerm = "",
           long? id = null,
           long? groupId = null,
           long? menuId = null)
        {
            var query = context.GroupMenus
                        .AsNoTracking()
                        .Where(v =>
                            (string.IsNullOrEmpty(searchTerm) ||
                             EF.Functions.Like(v.Menu.Name, $"%{searchTerm}%") ||
                             EF.Functions.Like(v.Id.ToString(), $"%{searchTerm}%")) &&
                            (!id.HasValue || v.Id == id.Value) &&
                            (!groupId.HasValue || v.GroupId == groupId.Value) &&
                            (!menuId.HasValue || v.MenuId == menuId.Value));

            var pagedResult = query
                .OrderBy(x => x.Menu.Name)
                .Include(x => x.Menu)
                .Include(x => x.Group)
                .Include(x => x.Menu.Parent);

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

    public static class MyQuery
    {
        public static async Task<(dynamic, int)> LoadData(this System.Net.Http.IHttpClientFactory HttpClientFactory, string query, int pageIndex, int pageSize, string searchTerm, object? requestPayload = null)
        {
            try
            {
                var client = HttpClientFactory.CreateClient("GraphQLClient");

                requestPayload = requestPayload ?? new
                {
                    query,
                    variables = new
                    {
                        pageIndex = pageIndex + 1,
                        pageSize,
                        searchTerm
                    }
                };

                var response = await client.PostAsJsonAsync("", requestPayload);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonReturn = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    return (jsonReturn, response.StatusCode.ToInt32());
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    return (errorContent, response.StatusCode.ToInt32());

                    //throw new Exception($"Request failed with status code {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred: {ex.Message}");

                return (ex.Message, 500);
            }
        }

        public static async Task<(List<OccupationalDto>, int)> GetOccupationals(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    occupationals(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                        items {
                            id
                            name
                            description
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<OccupationalDto>>(JsonConvert.SerializeObject(result.Item1.data.occupationals.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.occupationals.activePageIndexCount);
        }

        public static async Task<(List<CountryDto>, int)> GetCountries(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    countries(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                        items {
                            id
                            name
                            code
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<CountryDto>>(JsonConvert.SerializeObject(result.Item1.data.countries.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.countries.activePageIndexCount);
        }

        public static async Task<(List<ProvinceDto>, int)> GetProvinces(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    provinces(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                        items {
                            id
                            name
                            code
                            country {
                                id
                                name
                            }
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<ProvinceDto>>(JsonConvert.SerializeObject(result.Item1.data.provinces.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.provinces.activePageIndexCount);
        }

        public static async Task<(List<CityDto>, int)> GetCities(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    cities(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                        items {
                            id
                            name
                            province {
                                id
                                name
                            }
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<CityDto>>(JsonConvert.SerializeObject(result.Item1.data.cities.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.cities.activePageIndexCount);
        }

        public static async Task<(List<DistrictDto>, int)> GetDistricts(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    districts(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                        items {
                            id
                            name
                           province {
                                    id
                                    name
                                }
                            city {
                                    id
                                    name
                                }
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<DistrictDto>>(JsonConvert.SerializeObject(result.Item1.data.districts.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.districts.activePageIndexCount);
        }

        public static async Task<(List<VillageDto>, int)> GetVillages(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    villages(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                        items {
                            id
                            name
                            province {
                                    id
                                    name
                                  }
                                  city {
                                    id
                                    name
                                  }
                                  district {
                                    id
                                    name
                                  }
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<VillageDto>>(JsonConvert.SerializeObject(result.Item1.data.villages.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.villages.activePageIndexCount);
        }

        public static async Task<(List<MenuDto>, int)> GetMenus(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    menus(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                       items {
                          id
                          name
                          sequence
                          url
                          icon
                          parentId
                          parent {
                            id
                            name
                          }
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<MenuDto>>(JsonConvert.SerializeObject(result.Item1.data.menus.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.menus.activePageIndexCount);
        }

        public static async Task<(List<GroupDto>, int)> GetGroups(this System.Net.Http.IHttpClientFactory HttpClientFactory, int pageIndex, int pageSize, string? search = "")
        {
            var query = @"
                query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!) {
                    groups(pageIndex: $pageIndex, pageSize: $pageSize, searchTerm: $searchTerm) {
                        activePageIndexCount
                       items {
                          id
                          name
                          isDefaultData
                        }
                    }
                }";

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "");

            return (JsonConvert.DeserializeObject<List<GroupDto>>(JsonConvert.SerializeObject(result.Item1.data.groups.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.groups.activePageIndexCount);
        }

        public static async Task<(List<GroupMenuDto>, int)> GetGroupMenus(this System.Net.Http.IHttpClientFactory HttpClientFactory,
             int pageIndex,
             int pageSize,
             string? search = "",
             long? id = null,
             long? menuId = null,
             long? groupId = null)
        {
            var query = @"
        query Query($pageIndex: Int!, $pageSize: Int!, $searchTerm: String!, $id: Long, $menuId: Long, $groupId: Long) {
            groupMenus(
                pageIndex: $pageIndex,
                pageSize: $pageSize,
                searchTerm: $searchTerm,
                id: $id,
                groupId: $groupId,
                menuId: $menuId
            ) {
                activePageIndexCount
                items {
                    id
                    isCreate
                    isDefaultData
                    isDelete
                    isImport
                    isRead
                    isUpdate
                    groupId
                    group {
                        id
                        isDefaultData
                        name
                    }
                    menuId
                    menu {
                        id
                        name
                        isDefaultData
                        parentId
                        parent{
                            id
                            name
                        }
                        sequence
                        icon
                        url
                    }
                }
            }
        }";

            var client = HttpClientFactory.CreateClient("GraphQLClient");

            var requestPayload = new
            {
                query,
                variables = new
                {
                    pageIndex = pageIndex + 1,
                    pageSize,
                    searchTerm = search,
                    id,
                    menuId,
                    groupId
                }
            };

            var result = await LoadData(HttpClientFactory, query, pageIndex, pageSize, search ?? "", requestPayload);

            return (JsonConvert.DeserializeObject<List<GroupMenuDto>>(JsonConvert.SerializeObject(result.Item1.data.groupMenus.items)),
                    JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(result.Item1)).data.groupMenus.activePageIndexCount);
        }
    }
}