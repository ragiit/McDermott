 public class GeneralConsultanMedicalSupportCommand
 {
     #region GET

     public class GetGeneralConsultanMedicalSupportQuery : IRequest<(List<GeneralConsultanMedicalSupportDto>, int PageIndex, int PageSize, int PageCount)>
     {
         public List<Expression<Func<GeneralConsultanMedicalSupport, object>>> Includes { get; set; }
         public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; set; }
         public Expression<Func<GeneralConsultanMedicalSupport, GeneralConsultanMedicalSupport>> Select { get; set; }

         public List<(Expression<Func<GeneralConsultanMedicalSupport, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

         public bool IsDescending { get; set; } = false; // default to ascending
         public int PageIndex { get; set; } = 0;
         public int PageSize { get; set; } = 10;
         public bool IsGetAll { get; set; } = false;
         public string SearchTerm { get; set; }
     }

     public class GetSingleGeneralConsultanMedicalSupportQuery : IRequest<GeneralConsultanMedicalSupportDto>
     {
         public List<Expression<Func<GeneralConsultanMedicalSupport, object>>> Includes { get; set; }
         public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; set; }
         public Expression<Func<GeneralConsultanMedicalSupport, GeneralConsultanMedicalSupport>> Select { get; set; }

         public List<(Expression<Func<GeneralConsultanMedicalSupport, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

         public bool IsDescending { get; set; } = false; // default to ascending
         public int PageIndex { get; set; } = 0;
         public int PageSize { get; set; } = 10;
         public bool IsGetAll { get; set; } = false;
         public string SearchTerm { get; set; }
     }

     public class ValidateGeneralConsultanMedicalSupport(Expression<Func<GeneralConsultanMedicalSupport, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<GeneralConsultanMedicalSupport, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<GeneralConsultanMedicalSupportDto>
     {
         public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
     }

     public class BulkValidateGeneralConsultanMedicalSupport(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportsToValidate) : IRequest<List<GeneralConsultanMedicalSupportDto>>
     {
         public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportsToValidate { get; } = GeneralConsultanMedicalSupportsToValidate;
     }

     public class CreateListGeneralConsultanMedicalSupportRequest(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos) : IRequest<List<GeneralConsultanMedicalSupportDto>>
     {
         public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos { get; set; } = GeneralConsultanMedicalSupportDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto) : IRequest<GeneralConsultanMedicalSupportDto>
     {
         public GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupportDto { get; set; } = GeneralConsultanMedicalSupportDto;
     }

     public class UpdateListGeneralConsultanMedicalSupportRequest(List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos) : IRequest<List<GeneralConsultanMedicalSupportDto>>
     {
         public List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupportDtos { get; set; } = GeneralConsultanMedicalSupportDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteGeneralConsultanMedicalSupportRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
     {
         public long Id { get; set; } = id ?? 0;
         public List<long> Ids { get; set; } = ids ?? [];
     }

     #endregion DELETE
 }

public class GeneralConsultanMedicalSupportHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetGeneralConsultanMedicalSupportQuery, (List<GeneralConsultanMedicalSupportDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleGeneralConsultanMedicalSupportQuery, GeneralConsultanMedicalSupportDto>, IRequestHandler<ValidateGeneralConsultanMedicalSupport, bool>,
     IRequestHandler<CreateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>,
     IRequestHandler<BulkValidateGeneralConsultanMedicalSupport, List<GeneralConsultanMedicalSupportDto>>,
     IRequestHandler<CreateListGeneralConsultanMedicalSupportRequest, List<GeneralConsultanMedicalSupportDto>>,
     IRequestHandler<UpdateGeneralConsultanMedicalSupportRequest, GeneralConsultanMedicalSupportDto>,
     IRequestHandler<UpdateListGeneralConsultanMedicalSupportRequest, List<GeneralConsultanMedicalSupportDto>>,
     IRequestHandler<DeleteGeneralConsultanMedicalSupportRequest, bool>
 {
     #region GET

     public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(BulkValidateGeneralConsultanMedicalSupport request, CancellationToken cancellationToken)
     {
         var GeneralConsultanMedicalSupportDtos = request.GeneralConsultanMedicalSupportsToValidate;

         // Ekstrak semua kombinasi yang akan dicari di database
         var GeneralConsultanMedicalSupportNames = GeneralConsultanMedicalSupportDtos.Select(x => x.Name).Distinct().ToList();
         var a = GeneralConsultanMedicalSupportDtos.Select(x => x.CountryId).Distinct().ToList();

         var existingGeneralConsultanMedicalSupports = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>()
             .Entities
             .AsNoTracking()
             .Where(v => GeneralConsultanMedicalSupportNames.Contains(v.Name)
                         && a.Contains(v.CountryId))
             .ToListAsync(cancellationToken);

         return existingGeneralConsultanMedicalSupports.Adapt<List<GeneralConsultanMedicalSupportDto>>();
     }

     public async Task<bool> Handle(ValidateGeneralConsultanMedicalSupport request, CancellationToken cancellationToken)
     {
         return await _unitOfWork.Repository<GeneralConsultanMedicalSupport>()
             .Entities
             .AsNoTracking()
             .Where(request.Predicate)  // Apply the Predicate for filtering
             .AnyAsync(cancellationToken);  // Check if any record matches the condition
     }

     public async Task<(List<GeneralConsultanMedicalSupportDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGeneralConsultanMedicalSupportQuery request, CancellationToken cancellationToken)
     {
         try
         {
             var query = _unitOfWork.Repository<GeneralConsultanMedicalSupport>().Entities.AsNoTracking();

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
                         ? ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenByDescending(additionalOrderBy.OrderBy)
                         : ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenBy(additionalOrderBy.OrderBy);
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
                         EF.Functions.Like(v.Country.Name, $"%{request.SearchTerm}%")
                         );
             }

             // Apply dynamic select if provided
             if (request.Select is not null)
                 query = query.Select(request.Select);
             else
                 query = query.Select(x => new GeneralConsultanMedicalSupport
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Code = x.Code,
                     CountryId = x.CountryId,
                     Country = new Country
                     {
                         Name = x.Country == null ? string.Empty : x.Country.Name
                     }
                 });

             if (!request.IsGetAll)
             { // Paginate and sort
                 var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                     query,
                     request.PageSize,
                     request.PageIndex,
                     cancellationToken
                 );

                 return (pagedItems.Adapt<List<GeneralConsultanMedicalSupportDto>>(), request.PageIndex, request.PageSize, totalPages);
             }
             else
             {
                 return ((await query.ToListAsync(cancellationToken)).Adapt<List<GeneralConsultanMedicalSupportDto>>(), 0, 1, 1);
             }
         }
         catch (Exception ex)
         {
             // Consider logging the exception
             throw;
         }
     }

     public async Task<GeneralConsultanMedicalSupportDto> Handle(GetSingleGeneralConsultanMedicalSupportQuery request, CancellationToken cancellationToken)
     {
         try
         {
             var query = _unitOfWork.Repository<GeneralConsultanMedicalSupport>().Entities.AsNoTracking();

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
                         ? ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenByDescending(additionalOrderBy.OrderBy)
                         : ((IOrderedQueryable<GeneralConsultanMedicalSupport>)query).ThenBy(additionalOrderBy.OrderBy);
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
                     EF.Functions.Like(v.Country.Name, $"%{request.SearchTerm}%")
                     );
             }

             // Apply dynamic select if provided
             if (request.Select is not null)
                 query = query.Select(request.Select);
             else
                 query = query.Select(x => new GeneralConsultanMedicalSupport
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Code = x.Code,
                     CountryId = x.CountryId,
                     Country = new Country
                     {
                         Name = x.Country == null ? string.Empty : x.Country.Name
                     }
                 });

             return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GeneralConsultanMedicalSupportDto>();
         }
         catch (Exception ex)
         {
             // Consider logging the exception
             throw;
         }
     }

     #endregion GET

     #region CREATE

     public async Task<GeneralConsultanMedicalSupportDto> Handle(CreateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDto.Adapt<CreateUpdateGeneralConsultanMedicalSupportDto>().Adapt<GeneralConsultanMedicalSupport>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GeneralConsultanMedicalSupportDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(CreateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().AddAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<GeneralConsultanMedicalSupportDto> Handle(UpdateGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(request.GeneralConsultanMedicalSupportDto.Adapt<GeneralConsultanMedicalSupportDto>().Adapt<GeneralConsultanMedicalSupport>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<GeneralConsultanMedicalSupportDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<GeneralConsultanMedicalSupportDto>> Handle(UpdateListGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().UpdateAsync(request.GeneralConsultanMedicalSupportDtos.Adapt<List<GeneralConsultanMedicalSupport>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<GeneralConsultanMedicalSupportDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteGeneralConsultanMedicalSupportRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<GeneralConsultanMedicalSupport>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetGeneralConsultanMedicalSupportQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
 }