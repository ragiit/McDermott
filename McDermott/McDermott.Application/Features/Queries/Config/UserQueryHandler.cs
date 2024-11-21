using McDermott.Application.Features.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace McDermott.Application.Features.Queries.Config
{
    public class UserQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetUserQuery, List<UserDto>>,
        IRequestHandler<GetUserQuerys, (List<UserDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<ValidateUserQuery, bool>,
        IRequestHandler<BulkValidateUserQuery, List<UserDto>>,
        IRequestHandler<BulkValidateEmployeeQuery, List<UserDto>>,
        IRequestHandler<BulkValidatePatientQuery, List<UserDto>>,
        IRequestHandler<GetUserQuery2, (List<UserDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetUserInfoGroupQuery, List<UserDto>>,
        IRequestHandler<GetDataUserForKioskQuery, List<UserDto>>,
        IRequestHandler<CreateUserRequest, UserDto>,
        IRequestHandler<GetSingleUserQuery, UserDto>,
        IRequestHandler<CreateListUserRequest, List<UserDto>>,
        IRequestHandler<UpdateUserRequest, UserDto>,
        IRequestHandler<DeleteUserRequest, bool>,

        IRequestHandler<GetUserQueryNew, (List<UserDto>, int pageIndex, int pageSize, int pageCount)>
    {
        #region Get

        public async Task<(List<UserDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetUserQueryNew request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<User>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<User>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<User>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                         EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.Group.Name, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.Department.Name, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.Supervisor.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new User
                    {
                        Id = x.Id,
                        Name = x.Name
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<UserDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<UserDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<List<UserDto>> Handle(BulkValidateEmployeeQuery request, CancellationToken cancellationToken)
        {
            var userDtos = request.UsersToValidate;

            // Get distinct values from UserDtos
            var userNames = userDtos.Select(x => x.Name).Distinct().ToList();
            var groupIds = userDtos.Select(x => x.GroupId).Distinct().ToList();
            var identityNumbers = userDtos.Select(x => x.NoId).Distinct().ToList();
            var religionIds = userDtos.Select(x => x.ReligionId).Distinct().ToList();
            var datesOfBirth = userDtos.Select(x => x.DateOfBirth).Distinct().ToList();
            var genders = userDtos.Select(x => x.Gender).Distinct().ToList();
            var martialStatuses = userDtos.Select(x => x.MartialStatus).Distinct().ToList();
            var mobilePhones = userDtos.Select(x => x.MobilePhone).Distinct().ToList();
            var currentMobiles = userDtos.Select(x => x.CurrentMobile).Distinct().ToList();
            var homePhoneNumbers = userDtos.Select(x => x.HomePhoneNumber).Distinct().ToList();
            var npwps = userDtos.Select(x => x.Npwp).Distinct().ToList();
            var emergencyNames = userDtos.Select(x => x.EmergencyName).Distinct().ToList();
            var emergencyEmails = userDtos.Select(x => x.EmergencyEmail).Distinct().ToList();
            var emergencyPhones = userDtos.Select(x => x.EmergencyPhone).Distinct().ToList();
            var isEmployees = userDtos.Select(x => x.IsEmployee).Distinct().ToList();
            var noBpjsKs = userDtos.Select(x => x.NoBpjsKs).Distinct().ToList();
            var noBpjsTk = userDtos.Select(x => x.NoBpjsTk).Distinct().ToList();
            var legacies = userDtos.Select(x => x.Legacy).Distinct().ToList();
            var saps = userDtos.Select(x => x.SAP).Distinct().ToList();
            var nips = userDtos.Select(x => x.NIP).Distinct().ToList();
            var oracles = userDtos.Select(x => x.Oracle).Distinct().ToList();
            var employeeTypes = userDtos.Select(x => x.EmployeeType).Distinct().ToList();
            var joinDates = userDtos.Select(x => x.JoinDate).Distinct().ToList();

            // Fetch existing users
            var existingUsers = await _unitOfWork.Repository<User>()
                .Entities
                .AsNoTracking()
                .Where(v => userNames.Contains(v.Name) &&
                            groupIds.Contains(v.GroupId) &&
                            identityNumbers.Contains(v.NoId) &&
                            religionIds.Contains(v.ReligionId) &&
                            datesOfBirth.Contains(v.DateOfBirth) &&
                            genders.Contains(v.Gender) &&
                            martialStatuses.Contains(v.MartialStatus) &&
                            mobilePhones.Contains(v.MobilePhone) &&
                            currentMobiles.Contains(v.CurrentMobile) &&
                            homePhoneNumbers.Contains(v.HomePhoneNumber) &&
                            npwps.Contains(v.Npwp) &&
                            emergencyNames.Contains(v.EmergencyName) &&
                            emergencyEmails.Contains(v.EmergencyEmail) &&
                            emergencyPhones.Contains(v.EmergencyPhone) &&
                            isEmployees.Contains(v.IsEmployee) && // Match by IsEmployee
                            noBpjsKs.Contains(v.NoBpjsKs) && // Match by NoBpjsKs
                            noBpjsTk.Contains(v.NoBpjsTk) && // Match by NoBpjsTk
                            legacies.Contains(v.Legacy) && // Match by Legacy
                            saps.Contains(v.SAP) && // Match by SAP
                            nips.Contains(v.NIP) && // Match by NIP
                            oracles.Contains(v.Oracle) && // Match by Oracle
                            employeeTypes.Contains(v.EmployeeType) && // Match by EmployeeType
                            joinDates.Contains(v.JoinDate) // Match by JoinDate
                )
                .ToListAsync(cancellationToken);

            return existingUsers.Adapt<List<UserDto>>();
        }

        public async Task<List<UserDto>> Handle(BulkValidatePatientQuery request, CancellationToken cancellationToken)
        {
            var userDtos = request.UsersToValidate;

            // Get distinct values from UserDtos
            var userNames = userDtos.Select(x => x.Name).Distinct().ToList();
            var emails = userDtos.Select(x => x.Email).Distinct().ToList();
            var religionIds = userDtos.Select(x => x.ReligionId).Distinct().ToList();
            var datesOfBirth = userDtos.Select(x => x.DateOfBirth).Distinct().ToList();
            var genders = userDtos.Select(x => x.Gender).Distinct().ToList();
            var martialStatuses = userDtos.Select(x => x.MartialStatus).Distinct().ToList();
            var mobilePhones = userDtos.Select(x => x.MobilePhone).Distinct().ToList();
            var currentMobiles = userDtos.Select(x => x.CurrentMobile).Distinct().ToList();
            var homePhoneNumbers = userDtos.Select(x => x.HomePhoneNumber).Distinct().ToList();
            var npwps = userDtos.Select(x => x.Npwp).Distinct().ToList();
            var emergencyNames = userDtos.Select(x => x.EmergencyName).Distinct().ToList();
            var emergencyEmails = userDtos.Select(x => x.EmergencyEmail).Distinct().ToList();
            var emergencyPhones = userDtos.Select(x => x.EmergencyPhone).Distinct().ToList();
            var isPatient = userDtos.Select(x => x.IsPatient).Distinct().ToList();
            var noIds = userDtos.Select(x => x.NoId).Distinct().ToList();

            // Fetch existing users
            var existingUsers = await _unitOfWork.Repository<User>()
                .Entities
                .AsNoTracking()
                .Where(v => userNames.Contains(v.Name) &&
                            emails.Contains(v.Email) &&
                            noIds.Contains(v.NoId) &&
                            religionIds.Contains(v.ReligionId) &&
                            datesOfBirth.Contains(v.DateOfBirth) &&
                            genders.Contains(v.Gender) &&
                            martialStatuses.Contains(v.MartialStatus) &&
                            mobilePhones.Contains(v.MobilePhone) &&
                            currentMobiles.Contains(v.CurrentMobile) &&
                            homePhoneNumbers.Contains(v.HomePhoneNumber) &&
                            npwps.Contains(v.Npwp) &&
                            emergencyNames.Contains(v.EmergencyName) &&
                            emergencyEmails.Contains(v.EmergencyEmail) &&
                            emergencyPhones.Contains(v.EmergencyPhone) &&
                            isPatient.Contains(v.IsPatient)
                )
                .ToListAsync(cancellationToken);

            return existingUsers.Adapt<List<UserDto>>();
        }

        public async Task<List<UserDto>> Handle(BulkValidatePractitionerQuery request, CancellationToken cancellationToken)
        {
            var userDtos = request.UsersToValidate;

            // Get distinct values from UserDtos
            var userNames = userDtos.Select(x => x.Name).Distinct().ToList();
            var emails = userDtos.Select(x => x.Email).Distinct().ToList();
            var religionIds = userDtos.Select(x => x.ReligionId).Distinct().ToList();
            var datesOfBirth = userDtos.Select(x => x.DateOfBirth).Distinct().ToList();
            var genders = userDtos.Select(x => x.Gender).Distinct().ToList();
            var martialStatuses = userDtos.Select(x => x.MartialStatus).Distinct().ToList();
            var mobilePhones = userDtos.Select(x => x.MobilePhone).Distinct().ToList();
            var currentMobiles = userDtos.Select(x => x.CurrentMobile).Distinct().ToList();
            var homePhoneNumbers = userDtos.Select(x => x.HomePhoneNumber).Distinct().ToList();
            var npwps = userDtos.Select(x => x.Npwp).Distinct().ToList();
            var emergencyNames = userDtos.Select(x => x.EmergencyName).Distinct().ToList();
            var emergencyEmails = userDtos.Select(x => x.EmergencyEmail).Distinct().ToList();
            var emergencyPhones = userDtos.Select(x => x.EmergencyPhone).Distinct().ToList();
            var isPhysicion = userDtos.Select(x => x.IsPhysicion).Distinct().ToList();
            var isNurse = userDtos.Select(x => x.IsNurse).Distinct().ToList();
            var sips = userDtos.Select(x => x.SipNo).Distinct().ToList();
            var sipExps = userDtos.Select(x => x.SipExp).Distinct().ToList();
            var strs = userDtos.Select(x => x.StrNo).Distinct().ToList();
            var strExps = userDtos.Select(x => x.StrExp).Distinct().ToList();
            var noIds = userDtos.Select(x => x.NoId).Distinct().ToList();

            // Fetch existing users
            var existingUsers = await _unitOfWork.Repository<User>()
                .Entities
                .AsNoTracking()
                .Where(v => userNames.Contains(v.Name) &&
                            emails.Contains(v.Email) &&
                            noIds.Contains(v.NoId) &&
                            religionIds.Contains(v.ReligionId) &&
                            datesOfBirth.Contains(v.DateOfBirth) &&
                            genders.Contains(v.Gender) &&
                            martialStatuses.Contains(v.MartialStatus) &&
                            mobilePhones.Contains(v.MobilePhone) &&
                            currentMobiles.Contains(v.CurrentMobile) &&
                            homePhoneNumbers.Contains(v.HomePhoneNumber) &&
                            npwps.Contains(v.Npwp) &&
                            emergencyNames.Contains(v.EmergencyName) &&
                            emergencyEmails.Contains(v.EmergencyEmail) &&
                            emergencyPhones.Contains(v.EmergencyPhone) &&
                            isPhysicion.Contains(v.IsPatient) &&
                            isNurse.Contains(v.IsPatient) &&
                            sips.Contains(v.SipNo) &&
                            sipExps.Contains(v.SipExp) &&
                            strs.Contains(v.StrNo) &&
                            strExps.Contains(v.StrExp)
                )
                .ToListAsync(cancellationToken);

            return existingUsers.Adapt<List<UserDto>>();
        }

        public async Task<List<UserDto>> Handle(BulkValidateUserQuery request, CancellationToken cancellationToken)
        {
            var userDtos = request.UsersToValidate;

            // Get distinct values from UserDtos
            var userNames = userDtos.Select(x => x.Name).Distinct().ToList();
            var groupIds = userDtos.Select(x => x.GroupId).Distinct().ToList();
            var identityNumbers = userDtos.Select(x => x.NoId).Distinct().ToList();
            var religionIds = userDtos.Select(x => x.ReligionId).Distinct().ToList();
            var datesOfBirth = userDtos.Select(x => x.DateOfBirth).Distinct().ToList();
            var genders = userDtos.Select(x => x.Gender).Distinct().ToList();
            var martialStatuses = userDtos.Select(x => x.MartialStatus).Distinct().ToList();
            var mobilePhones = userDtos.Select(x => x.MobilePhone).Distinct().ToList();
            var currentMobiles = userDtos.Select(x => x.CurrentMobile).Distinct().ToList();
            var homePhoneNumbers = userDtos.Select(x => x.HomePhoneNumber).Distinct().ToList();
            var npwps = userDtos.Select(x => x.Npwp).Distinct().ToList();
            var emergencyNames = userDtos.Select(x => x.EmergencyName).Distinct().ToList();
            var emergencyEmails = userDtos.Select(x => x.EmergencyEmail).Distinct().ToList();
            var emergencyPhones = userDtos.Select(x => x.EmergencyPhone).Distinct().ToList();

            // Fetch existing users
            var existingUsers = await _unitOfWork.Repository<User>()
                .Entities
                .AsNoTracking()
                .Where(v => userNames.Contains(v.Name) &&
                            groupIds.Contains(v.GroupId) && // Assuming GroupId corresponds to your DTO
                            identityNumbers.Contains(v.NoId) && // Match by NoId
                            religionIds.Contains(v.ReligionId) && // Match by ReligionId
                            datesOfBirth.Contains(v.DateOfBirth) && // Match by DateOfBirth
                            genders.Contains(v.Gender) && // Match by Gender
                            martialStatuses.Contains(v.MartialStatus) && // Match by MartialStatus
                            mobilePhones.Contains(v.MobilePhone) && // Match by MobilePhone
                            currentMobiles.Contains(v.CurrentMobile) && // Match by CurrentMobile
                            homePhoneNumbers.Contains(v.HomePhoneNumber) && // Match by HomePhoneNumber
                            npwps.Contains(v.Npwp) && // Match by Npwp
                            emergencyNames.Contains(v.EmergencyName) && // Match by EmergencyName
                            emergencyEmails.Contains(v.EmergencyEmail) && // Match by EmergencyEmail
                            emergencyPhones.Contains(v.EmergencyPhone) // Match by EmergencyPhone
                )
                .ToListAsync(cancellationToken);

            return existingUsers.Adapt<List<UserDto>>();
        }

        public async Task<(List<UserDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetUserQuerys request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<User>().Entities
                    .AsNoTracking()
                    .Include(x => x.Group)
                    .Include(x => x.Supervisor)
                    .Include(x => x.Department)
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%")
                        //||
                        //EF.Functions.Like(v.Province.Name, $"%{request.SearchTerm}%")
                        );
                }

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var (totalCount, paged, totalPages) = await PaginateAsyncClass.PaginateAsync(request.PageSize, request.PageIndex, query, pagedResult, cancellationToken);

                return (paged.Adapt<List<UserDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<User>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<UserDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetUserQuery2 request, CancellationToken cancellationToken)
        {
            try
            {
                //var query = _unitOfWork.Repository<User>().Entities
                //    .AsNoTracking()
                //    .Include(x => x.Group)
                //    .Include(x => x.Supervisor)
                //    .Include(x => x.Department)
                //    .AsQueryable();

                var query = _unitOfWork.Repository<User>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Group.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Department.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Supervisor.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<UserDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                        .Include(x => x.Department)
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
                        .Include(x => x.Supervisor)
                        .Include(x => x.Department)
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
                    if (result.Any(x => !string.IsNullOrWhiteSpace(x.Legacy) && x.Legacy.Equals(request.Number)))
                    {
                        type = "Legacy";
                    }
                    else if (result.Any(x => !string.IsNullOrWhiteSpace(x.NIP) && x.NIP.Equals(request.Number)))
                    {
                        type = "NIP";
                    }
                    else if (result.Any(x => !string.IsNullOrWhiteSpace(x.Oracle) && x.Oracle.Equals(request.Number)))
                    {
                        type = "Oracle";
                    }
                    else if (result.Any(x => !string.IsNullOrWhiteSpace(x.SAP) && x.SAP.Equals(request.Number)))
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
                        var asiop = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Id.Equals(idUser));

                        // Create a list to accumulate family members' data
                        List<UserDto> familyMembersData = [];

                        //foreach (var relation in asiop)
                        //{
                        //    // Fetch family members based on the relation's FamilyMemberId
                        //    var familyMembers = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Id.Equals(relation.FamilyMemberId));
                        //    var FamilyId = await _unitOfWork.Repository<Family>().GetAllAsync(x => x.Id.Equals(relation.FamilyId));
                        //    var nameFamily = FamilyId.Select(x => x.Name).FirstOrDefault();

                        //    // Convert the family members to UserDto and add to the family members' data list
                        //    var familyMemberDtos = familyMembers.Adapt<List<UserDto>>();
                        //    foreach (var familyMemberDto in familyMemberDtos)
                        //    {
                        //        familyMemberDto.FamilyRelation = nameFamily;  // Assuming the relation entity has a RelationshipType property
                        //    }
                        //    familyMembersData.AddRange(familyMemberDtos);
                        //}

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

                var req = request.UserDto.Adapt<CreateUpdateUserDto>();
                var result = await _unitOfWork.Repository<User>().AddAsync(req.Adapt<User>());

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

                var req = request.UserDto.Adapt<CreateUpdateUserDto>();
                var user = req.Adapt<User>();

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

        public async Task<List<UserDto>> Handle(CreateListUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<User>().AddAsync(request.UserDtos.Adapt<List<CreateUpdateUserDto>>().Adapt<List<User>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUserQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UserDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<User>().Entities.AsNoTracking();

                // Apply custom order by if provided
                if (request.OrderBy is not null)
                {
                    query = request.IsDescending ?
                        query.OrderByDescending(request.OrderBy) :
                        query.OrderBy(request.OrderBy);
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                // Return the first result as UserDto
                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<UserDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion Delete
    }
}