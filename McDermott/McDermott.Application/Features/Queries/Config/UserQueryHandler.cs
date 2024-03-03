using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using static McDermott.Application.Features.Commands.Config.CountryCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class UserQueryHandler
    {
        #region Get

        internal class GetAllUserQueryHandler(IUnitOfWork _unitOfWork, IDistributedCache _cache) : IRequestHandler<GetUserQuery, List<UserDto>>
        {
            // Method untuk menghapus cache
            private async Task RemoveCache(string cacheKey)
            {
                await _cache.RemoveAsync(cacheKey);
            }

            // Method untuk menambahkan atau memperbarui cache
            private async Task AddOrUpdateCache(string cacheKey, List<UserDto> data)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Contoh: Cache akan kedaluwarsa dalam 10 menit
                };
                await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)), cacheOptions);
            }

            // Method untuk menangani operasi create, update, dan delete
            private async Task HandleCacheOperations()
            {
                var cacheKey = "GetAllUsersQuery"; // Kunci cache untuk query ini
                await RemoveCache(cacheKey); // Hapus cache setelah melakukan operasi create, update, atau delete
            }


            public async Task<List<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
            {
                var cacheKey = "GetAllUsersQuery"; // Kunci cache untuk query ini

                // Cek apakah hasil query sudah ada di cache
                var cachedData = await _cache.GetAsync(cacheKey);
                if (cachedData != null)
                {
                    // Jika ada, kembalikan data dari cache
                    var cachedUsers = JsonSerializer.Deserialize<List<UserDto>>(cachedData);
                    return cachedUsers;
                }

                // Jika data tidak ada di cache, lakukan query ke database
                var users = await _unitOfWork.Repository<User>()
                    .Entities
                    .Include(x => x.Group)
                    .Select(User => User.Adapt<UserDto>())
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                // Simpan data ke cache
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Contoh: Cache akan kedaluwarsa dalam 10 menit
                };

                await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(users)), cacheOptions);

                return users;
            }

            public async Task<bool> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                // nanti dihapus
                request.UserDto.UserName = request.UserDto.Email;

                var user = request.UserDto.Adapt<User>();

                await _unitOfWork.Repository<User>().UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await HandleCacheOperations();

                return true;
            }
        }

        internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<User>().GetByIdAsync(request.Id);

                return result.Adapt<UserDto>();
            }
        }

        internal class GetUserByEmailPasswordQueryHandler : IRequestHandler<GetUserByEmailPasswordQuery, UserDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserByEmailPasswordQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserDto> Handle(GetUserByEmailPasswordQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Email.Equals(request.UserDto.Email) && x.Password.Equals(request.UserDto.Password));

                return result.Adapt<List<UserDto>>().FirstOrDefault()!;
            }
        }

        internal class GetUserForKioskQueryhandler : IRequestHandler<GetDataUserForKioskQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserForKioskQueryhandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<UserDto>> Handle(GetDataUserForKioskQuery request, CancellationToken cancellationToken)
            {
                List<UserDto> data = new List<UserDto>();
                if (request.Types == "Legacy")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Legacy!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }
                else if (request.Types == "Oracle")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Oracle!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }
                else if (request.Types == "SAP")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.SAP!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }
                else if (request.Types == "NIP")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.NIP!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }
                else if (request.Types == "NIK")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.NoId!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }
                return data;
            }
        }

        #endregion Get

        #region Create

        internal class CreateUserHandler : IRequestHandler<CreateUserRequest, UserDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateUserHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    if (!request.UserDto.TypeId.Contains("VISA"))
                        request.UserDto.ExpiredId = null;

                    // nanti dihapus
                    request.UserDto.UserName = request.UserDto.Email;

                    var result = await _unitOfWork.Repository<User>().AddAsync(request.UserDto.Adapt<User>());

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return result.Adapt<UserDto>();
                }
                catch (Exception e)
                {
                    Console.Write("😋" + e.Message);
                    throw;
                }
            }
        }

        #endregion Create

        #region Update

        //internal class UpdateUserHandler : IRequestHandler<UpdateUserRequest, bool>
        //{
        //    private readonly IUnitOfWork _unitOfWork;

        //    public UpdateUserHandler(IUnitOfWork unitOfWork)
        //    {
        //        _unitOfWork = unitOfWork;
        //    }

        //    public async Task<bool> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        //    {
        //        // nanti dihapus
        //        request.UserDto.UserName = request.UserDto.Email;

        //        var user = request.UserDto.Adapt<User>();

        //        await _unitOfWork.Repository<User>().UpdateAsync(user);
        //        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //        return true;
        //    }
        //}

        #endregion Update

        #region Delete

        internal class DeleteUserHandler : IRequestHandler<DeleteUserRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteUserHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<User>().DeleteAsync(request.Id);
                    await _unitOfWork.Repository<PatientAllergy>().DeleteAsync(x => x.UserId == request.Id);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        internal class DeleteListUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteListUserRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(DeleteListUserRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<User>().DeleteAsync(request.Id);
                    await _unitOfWork.Repository<PatientAllergy>().DeleteAsync(x => request.Id.Contains(x.UserId));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        internal class DeleteListCountryHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteListCountryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<bool> Handle(DeleteListCountryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        #endregion Delete
    }
}