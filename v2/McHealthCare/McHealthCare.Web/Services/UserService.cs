using McHealthCare.Context;
using McHealthCare.Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace McHealthCare.Web.Services
{
    public class UserService(AuthenticationStateProvider authenticationStateProvider, ApplicationDbContext context, IMemoryCache cache)
    {
        public async Task<ClaimsPrincipal> GetCurrentUserAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var user = await GetCurrentUserAsync();
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdString, out var userId) ? userId : Guid.Empty;
        }

        public async Task<ApplicationUser?> GetCurrentUserFromDatabaseAsync()
        {
            var userId = await GetCurrentUserIdAsync();

            // Kunci cache untuk menyimpan data pengguna
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out ApplicationUser? user))
            {
                user = await context.Users
                                     .Include(u => u.Group)
                                     .ThenInclude(u => u.GroupMenus)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(u => u.Id == userId.ToString());

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user;
        }

        public void RemoveUserFromCache(Guid userId)
        {
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";
            cache.Remove(cacheKey);
        }

        public async Task<string?> GetCurrentUserNameAsync()
        {
            var user = await GetCurrentUserAsync();
            return user.Identity?.Name;
        }

        public async Task<string?> GetCurrentUserEmailAsync()
        {
            var user = await GetCurrentUserAsync();
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            var user = await GetCurrentUserAsync();
            return user.Identity?.IsAuthenticated ?? false;
        }

        // Tambahkan metode lain sesuai kebutuhan, seperti mendapatkan roles atau klaim lainnya
    }
}