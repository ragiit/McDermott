using Microsoft.Extensions.Caching.Distributed;

namespace McDermott.Application.Features.Queries.Config
{
    public class UserQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetUserQuery, List<UserDto>>,
        IRequestHandler<GetUserInfoGroupQuery, List<UserDto>>,
        IRequestHandler<GetDataUserForKioskQuery, List<UserDto>>,
        IRequestHandler<CreateUserRequest, UserDto>,
        IRequestHandler<UpdateUserRequest, UserDto>,
        IRequestHandler<DeleteUserRequest, bool>
    {
        #region Get

        public async Task<List<UserDto>> Handle(GetUserInfoGroupQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetUserQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique
                if (!_cache.TryGetValue(cacheKey, out List<User>? result))
                {
                    //result = await _unitOfWork.Repository<User>().GetAsync(
                    //    null,
                    //    includes: x => x.Include(z => z.Group),
                    //    cancellationToken);
                    result = await _unitOfWork.Repository<User>().Entities
                        .Include(x => x.Group)
                        .Include(x => x.Supervisor)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

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

        public async Task<List<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetUserQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<User>? result))
                {
                    result = await _unitOfWork.Repository<User>().Entities
                        .Include(x => x.Group)
                        .Include(x => x.Gender)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

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

                // Fetch users based on the request number
                var result = await _unitOfWork.Repository<User>().GetAllAsync(
                    x => x.Legacy!.Equals(request.Number) ||
                         x.NIP!.Equals(request.Number) ||
                         x.Oracle!.Equals(request.Number) ||
                         x.SAP!.Equals(request.Number));

                if (result.Count > 0)
                {
                    string type = null;
                    if (result.Any(x => x.Legacy!.Equals(request.Number)))
                    {
                        type = "Legacy";
                    }
                    else if (result.Any(x => x.NIP!.Equals(request.Number)))
                    {
                        type = "NIP";
                    }
                    else if (result.Any(x => x.Oracle!.Equals(request.Number)))
                    {
                        type = "Oracle";
                    }
                    else if (result.Any(x => x.SAP!.Equals(request.Number)))
                    {
                        type = "SAP";
                    }
                    // Convert the result to UserDto and add to the data list
                    var userDtos = result.Adapt<List<UserDto>>();
                    foreach (var userDto in userDtos)
                    {
                        // Set RelationshipType to "Employee" for userDtos
                        userDto.FamilyRelation = "Employee";
                        userDto.TypeNumber = type;
                        userDto.Numbers = request.Number;
                    }
                    data.AddRange(userDtos);

                    // Get the first user's ID
                    var idUser = result.Select(x => x.Id).FirstOrDefault();
                    if (idUser != null)
                    {
                        // Fetch patient-family relations for the user
                        var asiop = await _unitOfWork.Repository<PatientFamilyRelation>().GetAllAsync(x => x.PatientId.Equals(idUser));

                        // Create a list to accumulate family members' data
                        List<UserDto> familyMembersData = new List<UserDto>();

                        foreach (var relation in asiop)
                        {
                            // Fetch family members based on the relation's FamilyMemberId
                            var familyMembers = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Id.Equals(relation.FamilyMemberId));
                            var FamilyId = await _unitOfWork.Repository<Family>().GetAllAsync(x => x.Id.Equals(relation.FamilyId));
                            var nameFamily = FamilyId.Select(x => x.Name).FirstOrDefault();

                            // Convert the family members to UserDto and add to the family members' data list
                            var familyMemberDtos = familyMembers.Adapt<List<UserDto>>();
                            foreach (var familyMemberDto in familyMemberDtos)
                            {
                                familyMemberDto.FamilyRelation = nameFamily;  // Assuming the relation entity has a RelationshipType property
                            }
                            familyMembersData.AddRange(familyMemberDtos);
                        }

                        // Add the accumulated family members' data to the main data list
                        data.AddRange(familyMembersData);
                    }
                }
                else
                {
                    var dataBPJS = await _unitOfWork.Repository<InsurancePolicy>().GetAllAsync(x => x.PolicyNumber!.Equals(request.Number));
                    var bpjsId = dataBPJS.Select(x => x.UserId).FirstOrDefault();
                    var cekUser = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Id!.Equals(bpjsId));
                    data = cekUser.Adapt<List<UserDto>>().ToList();
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
                _cache.Remove("GetGeneralConsultanServiceQuery_");

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
                _cache.Remove("GetGeneralConsultanServiceQuery_");

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
                _cache.Remove("GetGeneralConsultanServiceQuery_");

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
                _cache.Remove("GetGeneralConsultanServiceQuery_");

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