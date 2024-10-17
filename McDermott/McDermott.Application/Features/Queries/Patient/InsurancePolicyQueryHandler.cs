using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Patient
{
    public class InsurancePolicyQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInsurancePolicyQuery, (List<InsurancePolicyDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleInsurancePolicyQuery, InsurancePolicyDto>,
                IRequestHandler<ValidateInsurancePolicyQuery, bool>,
        IRequestHandler<CreateInsurancePolicyRequest, InsurancePolicyDto>,
        IRequestHandler<CreateListInsurancePolicyRequest, List<InsurancePolicyDto>>,
        IRequestHandler<UpdateInsurancePolicyRequest, InsurancePolicyDto>,
        IRequestHandler<UpdateListInsurancePolicyRequest, List<InsurancePolicyDto>>,
        IRequestHandler<DeleteInsurancePolicyRequest, bool>
    {
        #region GET

        public async Task<bool> Handle(ValidateInsurancePolicyQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<InsurancePolicy>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<InsurancePolicyDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetInsurancePolicyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<InsurancePolicy>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<InsurancePolicy>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<InsurancePolicy>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new InsurancePolicy
                    {
                        Id = x.Id,
                        UserId = x.UserId, // Patient
                        User = new User
                        {
                            Name = x.User == null ? string.Empty : x.User.Name,
                        },
                        InsuranceId = x.InsuranceId,
                        Insurance = new Insurance
                        {
                            Name = x.Insurance == null ? string.Empty : x.Insurance.Name,
                        },
                        PolicyNumber = x.PolicyNumber,
                        Active = x.Active,

                        // BPJS Integration fields
                        NoKartu = x.NoKartu,
                        Nama = x.Nama,
                        HubunganKeluarga = x.HubunganKeluarga,
                        TglLahir = x.TglLahir,
                        TglMulaiAktif = x.TglMulaiAktif,
                        TglAkhirBerlaku = x.TglAkhirBerlaku,
                        GolDarah = x.GolDarah,
                        NoHP = x.NoHP,
                        NoKTP = x.NoKTP,
                        PstProl = x.PstProl,
                        PstPrb = x.PstPrb,
                        Aktif = x.Aktif,
                        KetAktif = x.KetAktif,
                        Tunggakan = x.Tunggakan,
                        KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                        KdProviderPstNmProvider = x.KdProviderPstNmProvider,
                        KdProviderGigiKdProvider = x.KdProviderGigiKdProvider,
                        KdProviderGigiNmProvider = x.KdProviderGigiNmProvider,
                        JnsKelasNama = x.JnsKelasNama,
                        JnsKelasKode = x.JnsKelasKode,
                        JnsPesertaNama = x.JnsPesertaNama,
                        JnsPesertaKode = x.JnsPesertaKode,
                        AsuransiKdAsuransi = x.AsuransiKdAsuransi,
                        AsuransiNmAsuransi = x.AsuransiNmAsuransi,
                        AsuransiNoAsuransi = x.AsuransiNoAsuransi,
                        AsuransiCob = x.AsuransiCob
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<InsurancePolicyDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<InsurancePolicyDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<InsurancePolicyDto> Handle(GetSingleInsurancePolicyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<InsurancePolicy>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<InsurancePolicy>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<InsurancePolicy>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Phycisian.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.UoM.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.FormDrug.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new InsurancePolicy
                    {
                        Id = x.Id,
                        UserId = x.UserId, // Patient
                        User = new User
                        {
                            Name = x.User == null ? string.Empty : x.User.Name,
                        },
                        InsuranceId = x.InsuranceId,
                        PolicyNumber = x.PolicyNumber,
                        Active = x.Active,

                        // BPJS Integration fields
                        NoKartu = x.NoKartu,
                        Nama = x.Nama,
                        HubunganKeluarga = x.HubunganKeluarga,
                        TglLahir = x.TglLahir,
                        TglMulaiAktif = x.TglMulaiAktif,
                        TglAkhirBerlaku = x.TglAkhirBerlaku,
                        GolDarah = x.GolDarah,
                        NoHP = x.NoHP,
                        NoKTP = x.NoKTP,
                        PstProl = x.PstProl,
                        PstPrb = x.PstPrb,
                        Aktif = x.Aktif,
                        KetAktif = x.KetAktif,
                        Tunggakan = x.Tunggakan,
                        KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                        KdProviderPstNmProvider = x.KdProviderPstNmProvider,
                        KdProviderGigiKdProvider = x.KdProviderGigiKdProvider,
                        KdProviderGigiNmProvider = x.KdProviderGigiNmProvider,
                        JnsKelasNama = x.JnsKelasNama,
                        JnsKelasKode = x.JnsKelasKode,
                        JnsPesertaNama = x.JnsPesertaNama,
                        JnsPesertaKode = x.JnsPesertaKode,
                        AsuransiKdAsuransi = x.AsuransiKdAsuransi,
                        AsuransiNmAsuransi = x.AsuransiNmAsuransi,
                        AsuransiNoAsuransi = x.AsuransiNoAsuransi,
                        AsuransiCob = x.AsuransiCob
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<InsurancePolicyDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<InsurancePolicyDto> Handle(CreateInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().AddAsync(request.InsurancePolicyDto.Adapt<InsurancePolicy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsurancePolicyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsurancePolicyDto>> Handle(CreateListInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().AddAsync(request.InsurancePolicyDtos.Adapt<List<InsurancePolicy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsurancePolicyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InsurancePolicyDto> Handle(UpdateInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().UpdateAsync(request.InsurancePolicyDto.Adapt<InsurancePolicy>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InsurancePolicyDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InsurancePolicyDto>> Handle(UpdateListInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InsurancePolicy>().UpdateAsync(request.InsurancePolicyDtos.Adapt<List<InsurancePolicy>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InsurancePolicyDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInsurancePolicyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<InsurancePolicy>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<InsurancePolicy>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInsurancePolicyQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
    }
}