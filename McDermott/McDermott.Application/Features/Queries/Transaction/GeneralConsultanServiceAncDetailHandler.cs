using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanServiceAncDetailCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceAncDetailHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetGeneralConsultanServiceAncDetailQuery, (List<GeneralConsultanServiceAncDetailDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleGeneralConsultanServiceAncDetailQuery, GeneralConsultanServiceAncDetailDto>,
     //IRequestHandler<ValidateGeneralConsultanServiceAncDetail, bool>,
     IRequestHandler<CreateGeneralConsultanServiceAncDetailRequest, GeneralConsultanServiceAncDetailDto>,
     //IRequestHandler<BulkValidateGeneralConsultanServiceAncDetail, List<GeneralConsultanServiceAncDetailDto>>,
     IRequestHandler<CreateListGeneralConsultanServiceAncDetailRequest, List<GeneralConsultanServiceAncDetailDto>>,
     IRequestHandler<UpdateGeneralConsultanServiceAncDetailRequest, GeneralConsultanServiceAncDetailDto>,
     IRequestHandler<UpdateListGeneralConsultanServiceAncDetailRequest, List<GeneralConsultanServiceAncDetailDto>>,
     IRequestHandler<DeleteGeneralConsultanServiceAncDetailRequest, bool>
    {
        #region GET

        public async Task<(List<GeneralConsultanServiceAncDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServiceAncDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GeneralConsultanServiceAncDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanServiceAncDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //        EF.Functions.Like(v.GeneralConsultanServiceAncDetail.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanServiceAncDetail
                    {
                        Id = x.Id,
                        Date = x.Date,
                        GeneralConsultanServiceAncId = x.GeneralConsultanServiceAncId,
                        Trimester = x.Trimester,
                        Complaint = x.Complaint,
                        KU = x.KU,
                        TD = x.TD,
                        BB = x.BB,
                        UK = x.UK,
                        TFU = x.TFU,
                        FetusPosition = x.FetusPosition,
                        DJJ = x.DJJ,
                        TT = x.TT,
                        IsReadOnly = x.IsReadOnly,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GeneralConsultanServiceAncDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanServiceAncDetailDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GeneralConsultanServiceAncDetailDto> Handle(GetSingleGeneralConsultanServiceAncDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GeneralConsultanServiceAncDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanServiceAncDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.GeneralConsultanServiceAncDetail.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanServiceAncDetail
                    {
                        Id = x.Id,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanServiceAncDetailDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        //public async Task<List<GeneralConsultanServiceAncDetailDto>> Handle(BulkValidateGeneralConsultanServiceAncDetailQuery request, CancellationToken cancellationToken)
        //{
        //    var GeneralConsultanServiceAncDetailDtos = request.GeneralConsultanServiceAncDetailsToValidate;

        //    // Ekstrak semua kombinasi yang akan dicari di database
        //    var GeneralConsultanServiceAncDetailNames = GeneralConsultanServiceAncDetailDtos.Select(x => x.Name).Distinct().ToList();
        //    var postalCodes = GeneralConsultanServiceAncDetailDtos.Select(x => x.PostalCode).Distinct().ToList();
        //    var provinceIds = GeneralConsultanServiceAncDetailDtos.Select(x => x.ProvinceId).Distinct().ToList();
        //    var cityIds = GeneralConsultanServiceAncDetailDtos.Select(x => x.CityId).Distinct().ToList();
        //    var GeneralConsultanServiceAncDetailIds = GeneralConsultanServiceAncDetailDtos.Select(x => x.GeneralConsultanServiceAncDetailId).Distinct().ToList();

        //    var existingGeneralConsultanServiceAncDetails = await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>()
        //        .Entities
        //        .AsNoTracking()
        //        .Where(v => GeneralConsultanServiceAncDetailNames.Contains(v.Name)
        //                    && postalCodes.Contains(v.PostalCode)
        //                    && provinceIds.Contains(v.ProvinceId)
        //                    && cityIds.Contains(v.CityId)
        //                    && GeneralConsultanServiceAncDetailIds.Contains(v.GeneralConsultanServiceAncDetailId))
        //        .ToListAsync(cancellationToken);

        //    return existingGeneralConsultanServiceAncDetails.Adapt<List<GeneralConsultanServiceAncDetailDto>>();
        //}

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanServiceAncDetailDto> Handle(CreateGeneralConsultanServiceAncDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var aa = request.GeneralConsultanServiceAncDetailDto.Adapt<CreateUpdateGeneralConsultanServiceAncDetailDto>().Adapt<GeneralConsultanServiceAncDetail>();

                //if (string.IsNullOrWhiteSpace(aa.Reference))
                //{
                //    // Fetch the latest sequence from the Reference field
                //    var lastSeq = await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>()
                //        .Entities
                //        .OrderByDescending(x => x.Reference)
                //        .Select(x => x.Reference)
                //        .FirstOrDefaultAsync();

                //    int lastSequenceNumber = 0;

                //    // Check if a Reference was found, then extract the last sequence part
                //    if (!string.IsNullOrEmpty(lastSeq))
                //    {
                //        // Assuming the format is always consistent: ANC/YYYY/MM/DD/PatientId/Sequence
                //        var parts = lastSeq.Split('/');
                //        if (parts.Length > 4 && int.TryParse(parts[^1], out int parsedSequence))
                //        {
                //            lastSequenceNumber = parsedSequence; // The last part should be the sequence number
                //        }
                //    }

                //    // Now you have the last sequence number in `lastSequenceNumber`
                //    int newSequence = lastSequenceNumber + 1;

                //    string year = DateTime.Now.ToString("yyyy");
                //    string month = DateTime.Now.ToString("MM");
                //    string day = DateTime.Now.ToString("dd");
                //    int sequence = lastSequenceNumber + 1;

                //    aa.Reference = $"ANC/{year}/{month}/{day}/{aa.PatientId}/{sequence}";
                //}

                var result = await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().AddAsync(aa);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceAncDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceAncDetailDto>> Handle(CreateListGeneralConsultanServiceAncDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().AddAsync(request.GeneralConsultanServiceAncDetailDtos.Adapt<List<GeneralConsultanServiceAncDetail>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanServiceAncDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GeneralConsultanServiceAncDetailDto> Handle(UpdateGeneralConsultanServiceAncDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().UpdateAsync(request.GeneralConsultanServiceAncDetailDto.Adapt<GeneralConsultanServiceAncDetailDto>().Adapt<GeneralConsultanServiceAncDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceAncDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceAncDetailDto>> Handle(UpdateListGeneralConsultanServiceAncDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().UpdateAsync(request.GeneralConsultanServiceAncDetailDtos.Adapt<List<GeneralConsultanServiceAncDetail>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanServiceAncDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGeneralConsultanServiceAncDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanServiceAncDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

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