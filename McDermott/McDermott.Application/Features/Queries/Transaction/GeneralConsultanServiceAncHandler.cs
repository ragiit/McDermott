using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Transaction.GeneralConsultanServiceAncCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceAncHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
      IRequestHandler<GetGeneralConsultanServiceAncQuery, (List<GeneralConsultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)>,
      IRequestHandler<GetSingleGeneralConsultanServiceAncQuery, GeneralConsultanServiceAncDto>, IRequestHandler<ValidateGeneralConsultanServiceAnc, bool>,
      IRequestHandler<CreateGeneralConsultanServiceAncRequest, GeneralConsultanServiceAncDto>,
      IRequestHandler<BulkValidateGeneralConsultanServiceAnc, List<GeneralConsultanServiceAncDto>>,
      IRequestHandler<CreateListGeneralConsultanServiceAncRequest, List<GeneralConsultanServiceAncDto>>,
      IRequestHandler<UpdateGeneralConsultanServiceAncRequest, GeneralConsultanServiceAncDto>,
      IRequestHandler<UpdateListGeneralConsultanServiceAncRequest, List<GeneralConsultanServiceAncDto>>,
      IRequestHandler<DeleteGeneralConsultanServiceAncRequest, bool>
    {
        #region GET

        public async Task<List<GeneralConsultanServiceAncDto>> Handle(BulkValidateGeneralConsultanServiceAnc request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.GeneralConsultanServiceAncsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
            //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

            //var existingCountrys = await _unitOfWork.Repository<Country>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
            //    .ToListAsync(cancellationToken);

            //return existingCountrys.Adapt<List<CountryDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateGeneralConsultanServiceAnc request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GeneralConsultanServiceAnc>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GeneralConsultanServiceAncDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanServiceAncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanServiceAnc>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.GeneralConsultanServiceAnc.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanServiceAnc
                    {
                        Id = x.Id,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        Reference = x.Reference,
                        PregnancyStatusA = x.PregnancyStatusA,
                        PregnancyStatusG = x.PregnancyStatusG,
                        PregnancyStatusP = x.PregnancyStatusP,
                        HPHT = x.HPHT,
                        HPL = x.HPL,
                        LILA = x.LILA,
                        PatientId = x.PatientId
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<GeneralConsultanServiceAncDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanServiceAncDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GeneralConsultanServiceAncDto> Handle(GetSingleGeneralConsultanServiceAncQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GeneralConsultanServiceAnc>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GeneralConsultanServiceAnc>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.GeneralConsultanServiceAnc.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GeneralConsultanServiceAnc
                    {
                        Id = x.Id,
                        GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                        Reference = x.Reference,
                        PregnancyStatusA = x.PregnancyStatusA,
                        PregnancyStatusG = x.PregnancyStatusG,
                        PregnancyStatusP = x.PregnancyStatusP,
                        HPHT = x.HPHT,
                        HPL = x.HPL,
                        LILA = x.LILA,
                        PatientId = x.PatientId
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanServiceAncDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanServiceAncDto> Handle(CreateGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var aa = request.GeneralConsultanServiceAncDto.Adapt<CreateUpdateGeneralConsultanServiceAncDto>().Adapt<GeneralConsultanServiceAnc>();

                if (string.IsNullOrWhiteSpace(aa.Reference))
                {
                    // Fetch the latest sequence from the Reference field
                    var lastSeq = await _unitOfWork.Repository<GeneralConsultanServiceAnc>()
                        .Entities
                        .OrderByDescending(x => x.Reference)
                        .Select(x => x.Reference)
                        .FirstOrDefaultAsync();

                    int lastSequenceNumber = 0;

                    // Check if a Reference was found, then extract the last sequence part
                    if (!string.IsNullOrEmpty(lastSeq))
                    {
                        // Assuming the format is always consistent: ANC/YYYY/MM/DD/PatientId/Sequence
                        var parts = lastSeq.Split('/');
                        if (parts.Length > 4 && int.TryParse(parts[^1], out int parsedSequence))
                        {
                            lastSequenceNumber = parsedSequence; // The last part should be the sequence number
                        }
                    }

                    // Now you have the last sequence number in `lastSequenceNumber`
                    int newSequence = lastSequenceNumber + 1;

                    string year = DateTime.Now.ToString("yyyy");
                    string month = DateTime.Now.ToString("MM");
                    string day = DateTime.Now.ToString("dd");
                    int sequence = lastSequenceNumber + 1;

                    aa.Reference = $"ANC/{year}/{month}/{day}/{aa.PatientId}/{sequence}";
                }

                var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().AddAsync(aa);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceAncDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceAncDto>> Handle(CreateListGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().AddAsync(request.GeneralConsultanServiceAncDtos.Adapt<List<GeneralConsultanServiceAnc>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanServiceAncDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GeneralConsultanServiceAncDto> Handle(UpdateGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().UpdateAsync(request.GeneralConsultanServiceAncDto.Adapt<GeneralConsultanServiceAncDto>().Adapt<GeneralConsultanServiceAnc>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceAncDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceAncDto>> Handle(UpdateListGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanServiceAnc>().UpdateAsync(request.GeneralConsultanServiceAncDtos.Adapt<List<GeneralConsultanServiceAnc>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralConsultanServiceAncDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGeneralConsultanServiceAncRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanServiceAnc>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanServiceAnc>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceAncQuery_"); // Ganti dengan key yang sesuai

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