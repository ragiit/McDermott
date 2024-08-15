using Blazored.Toast.Services;
using McHealthCare.Context;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace McHealthCare.Web.Services
{
    public sealed class UserService(AuthenticationStateProvider authenticationStateProvider, IServiceScopeFactory _scopeFactory, NavigationManager navigationManager, IMemoryCache cache, IMediator mediator)
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

        public async Task<(bool, GroupMenuDto)> GetUserInfo(IToastService? toastService = null)
        {
            try
            {
                var url = navigationManager.Uri.ToLower().Replace(navigationManager.BaseUri.ToLower(), "");
                var user = await GetCurrentUserFromDatabaseAsync() ?? new();
                var groups = await mediator.Send(new GetGroupMenuQuery(x => x.GroupId == user.GroupId!)!);

                var userAccessCRUID = groups?.FirstOrDefault(x => x.Menu?.Url != null && url.Contains(x.Menu.Url.ToLower()));

                if (string.IsNullOrWhiteSpace(url) || userAccessCRUID != null || url == navigationManager.BaseUri)
                {
                    return (true, userAccessCRUID!);
                }

                return (false, new());

                navigationManager.NavigateTo("", true);
                toastService?.ClearErrorToasts();
                toastService?.ShowError("Unauthorized Access\r\n\r\nYou are not authorized to view this page. If you need access, please contact the administrator.\r\n");
            }
            catch (JSDisconnectedException)
            {
                navigationManager.NavigateTo("", true);
                return (false, new());
            }
        }

        public async Task<List<ApplicationUserDto>> GetAllUsers()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out List<ApplicationUser>? user))
            {
                user = await context.Users
                    .Include(x => x.Employee)
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                                     .Include(u => u.Group)
                                     .ThenInclude(u => u.GroupMenus)
                                     .AsNoTracking()
                                     .ToListAsync();

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user.Adapt<List<ApplicationUserDto>>() ?? [];
        }

        public async Task<List<Patient>?> GetAllPatients()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out List<Patient>? user))
            {
                user = await context.Patients
                                     .AsNoTracking()
                                     .ToListAsync();

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user ?? [];
        }

        public async Task<ApplicationUserDto> GetUserId(string userId, bool removeCache = false)
        {
            //using var dbContext = context.CreateDbContext();
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return (await context.Users.FindAsync(userId)).Adapt<ApplicationUserDto>();

            // Kunci cache untuk menyimpan data pengguna
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out ApplicationUser? user) || removeCache)
            {
                //user = await context.Users
                //                     .AsNoTracking()
                //                     .Include(u => u.Group)
                //                     .ThenInclude(u => u.GroupMenus)
                //                     .FirstOrDefaultAsync(u => u.Id == userId.ToString());

                user = await context.Users.FindAsync(userId);

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user.Adapt<ApplicationUserDto>() ?? new();
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(string userId, bool removeCache = false)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return (await context.Doctors.FindAsync(userId)).Adapt<DoctorDto>();
            // Kunci cache untuk menyimpan data pengguna
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out Doctor? user) || removeCache)
            {
                //user = await context.Users
                //                     .AsNoTracking()
                //                     .Include(u => u.Group)
                //                     .ThenInclude(u => u.GroupMenus)
                //                     .FirstOrDefaultAsync(u => u.Id == userId.ToString());

                user = await context.Doctors.FindAsync(userId);

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user.Adapt<DoctorDto>() ?? new();
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(string userId, bool removeCache = false)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return (await context.Employees.FindAsync(userId)).Adapt<EmployeeDto>();
            // Kunci cache untuk menyimpan data pengguna
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out Employee? user) || removeCache)
            {
                //user = await context.Users
                //                     .AsNoTracking()
                //                     .Include(u => u.Group)
                //                     .ThenInclude(u => u.GroupMenus)
                //                     .FirstOrDefaultAsync(u => u.Id == userId.ToString());

                user = await context.Employees.FindAsync(userId);

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user.Adapt<EmployeeDto>() ?? new();
        }

        public async Task<PatientDto> GetPatientByIdAsync(string userId, bool removeCache = false)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return (await context.Patients.FindAsync(userId)).Adapt<PatientDto>();
            // Kunci cache untuk menyimpan data pengguna
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";

            // Cek apakah data pengguna sudah ada di cache
            if (!cache.TryGetValue(cacheKey, out Patient? user) || removeCache)
            {
                //user = await context.Users
                //                     .AsNoTracking()
                //                     .Include(u => u.Group)
                //                     .ThenInclude(u => u.GroupMenus)
                //                     .FirstOrDefaultAsync(u => u.Id == userId.ToString());

                user = await context.Patients.FindAsync(userId);

                // Konfigurasi opsi caching
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Waktu kadaluarsa sliding
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));  // Waktu kadaluarsa absolut

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user, cacheEntryOptions);
            }

            return user.Adapt<PatientDto>() ?? new();
        }

        public async Task UpdateUserRolesAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, UserRoleDto userRoleDto)
        {
            var roles = Enum.GetValues(typeof(EnumRole)).Cast<EnumRole>();

            foreach (var role in roles)
            {
                string roleName = role.GetDisplayName();

                if (role == EnumRole.User && userRoleDto.IsUser)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.User && !userRoleDto.IsUser)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.Practitioner && userRoleDto.IsPractitioner)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.Practitioner && !userRoleDto.IsPractitioner)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.Patient && userRoleDto.IsPatient)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.Patient && !userRoleDto.IsPatient)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.MCU && userRoleDto.IsMCU)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.MCU && !userRoleDto.IsMCU)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.Employee && userRoleDto.IsEmployee)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.Employee && !userRoleDto.IsEmployee)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.Pharmacy && userRoleDto.IsPharmacy)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.Pharmacy && !userRoleDto.IsPharmacy)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.HR && userRoleDto.IsHR)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.HR && !userRoleDto.IsHR)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                if (role == EnumRole.Admin && userRoleDto.IsAdmin)
                {
                    if (!await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
                else if (role == EnumRole.Admin && !userRoleDto.IsAdmin)
                {
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        await userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }
            }
        }

        public async Task<UserRoleDto> GetUserRolesAsync(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            // Retrieve roles for the given user
            var roles = await userManager.GetRolesAsync(user);

            // Initialize UserRoleDto
            var userRoleDto = new UserRoleDto();

            // Map roles to UserRoleDto properties
            foreach (var role in roles)
            {
                if (role == EnumRole.HR.GetDisplayName())
                {
                    userRoleDto.IsHR = true;
                }
                else if (role == EnumRole.User.GetDisplayName())
                {
                    userRoleDto.IsUser = true;
                }
                else if (role == EnumRole.Admin.GetDisplayName())
                {
                    userRoleDto.IsAdmin = true;
                }
                else if (role == EnumRole.MCU.GetDisplayName())
                {
                    userRoleDto.IsMCU = true;
                }
                else if (role == EnumRole.Pharmacy.GetDisplayName())
                {
                    userRoleDto.IsPharmacy = true;
                }
                else if (role == EnumRole.Patient.GetDisplayName())
                {
                    userRoleDto.IsPatient = true;
                }
                else if (role == EnumRole.Practitioner.GetDisplayName())
                {
                    userRoleDto.IsPractitioner = true;
                }
                else if (role == EnumRole.Employee.GetDisplayName())
                {
                    userRoleDto.IsEmployee = true;
                }
            }

            return userRoleDto;
        }

        public async Task<ApplicationUser?> GetCurrentUserFromDatabaseAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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
                cache.Set(cacheKey, user);
            }

            if (user is null)
            {
                user = await context.Users
                                     .Include(u => u.Group)
                                     .ThenInclude(u => u.GroupMenus)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(u => u.Id == userId.ToString());

                // Simpan data pengguna dalam cache
                cache.Set(cacheKey, user);
            }

            return user;
        }

        public async Task RemoveUserDoctor(ApplicationUserDto userDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var u = await context.Doctors.FindAsync(userDto.Id);
            if (u != null)
            {
                context.Doctors.Remove(u);
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
        }

        public async Task RemovePatientDoctor(ApplicationUserDto userDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var u = await context.Patients.FindAsync(userDto.Id);
            if (u != null)
            {
                context.Patients.Remove(u);
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
        }

        public async Task RemoveEmployeeDoctor(ApplicationUserDto userDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var u = await context.Employees.FindAsync(userDto.Id);
            if (u != null)
            {
                context.Employees.Remove(u);
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
        }

        public async Task UpdateDoctorAsync(DoctorDto doctorDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var doctor = await context.Doctors.FindAsync(doctorDto.ApplicationUserId);
            var d = doctorDto.Adapt<CreateUpdateDoctorDto>();
            if (doctor != null)
            {
                // Mapster adaptation
                d.Adapt(doctor);
                context.Entry(doctor).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
            else
            {
                var a = d.Adapt<Doctor>();
                await context.Doctors.AddAsync(a);
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
        }

        public async Task UpdatePatientAsync(PatientDto patientDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var patient = await context.Patients.FindAsync(patientDto.ApplicationUserId);
            var d = patientDto.Adapt<CreateUpdatePatientDto>();
            if (patient != null)
            {
                // Mapster adaptation
                d.Adapt(patient);
                context.Entry(patient).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
            else
            {
                var a = d.Adapt<Patient>();
                await context.Patients.AddAsync(a);
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
        }

        public async Task UpdateEmployeeAsync(EmployeeDto employeeDto)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var employee = await context.Employees.FindAsync(employeeDto.ApplicationUserId);
            var d = employeeDto.Adapt<CreateUpdateEmployeeDto>();
            if (employee != null)
            {
                // Mapster adaptation
                d.Adapt(employee);
                context.Entry(employee).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
            else
            {
                var a = d.Adapt<Employee>();
                await context.Employees.AddAsync(a);
                await context.SaveChangesAsync();
                await RemoveUserFromCache();
            }
        }

        public async Task RemoveUserFromCache()
        {
            var userId = await GetCurrentUserIdAsync();
            string cacheKey = $"{CacheKey.UserCacheKeyPrefix}{userId}";
            string cacheKey1 = $"{CacheKey.UserCacheKeyPrefix}";
            cache.Remove(cacheKey);
            cache.Remove(cacheKey1);
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