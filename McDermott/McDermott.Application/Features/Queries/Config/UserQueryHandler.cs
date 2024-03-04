using Microsoft.Extensions.Caching.Distributed;

namespace McDermott.Application.Features.Queries.Config
{
    public class UserQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetUserQuery, List<UserDto>>,
        IRequestHandler<GetDataUserForKioskQuery, List<UserDto>>,
        IRequestHandler<CreateUserRequest, UserDto>,
        IRequestHandler<UpdateUserRequest, UserDto>,
        IRequestHandler<DeleteUserRequest, bool>
    {

        #region Get  
        public async Task<List<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetUserQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique
                if (!_cache.TryGetValue(cacheKey, out List<User>? result))
                {
                    result = await _unitOfWork.Repository<User>().GetAsync(
                        null,
                        includes: x => x.Include(z => z.Group),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<UserDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserDto>> Handle(GetDataUserForKioskQuery request, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Get

        #region Create
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

                _cache.Remove("GetUserQuery_");

                return result.Adapt<UserDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Create

        #region Update
        public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // nanti dihapus
                request.UserDto.UserName = request.UserDto.Email;

                var user = request.UserDto.Adapt<User>();

                var result = await _unitOfWork.Repository<User>().UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUserQuery_");

                return result.Adapt<UserDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserDto>> Handle(UpdateListUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<User>().UpdateAsync(request.UserDtos.Adapt<List<User>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUserQuery_");

                return result.Adapt<List<UserDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Update

        #region Delete
        public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<User>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<User>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUserQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Delete
    }
}