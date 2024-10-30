 public class BpjsClassificationCommand
 {
     #region GET

     public class GetBpjsClassificationQuery : IRequest<(List<BpjsClassificationDto>, int PageIndex, int PageSize, int PageCount)>
     {
         public List<Expression<Func<BpjsClassification, object>>> Includes { get; set; }
         public Expression<Func<BpjsClassification, bool>> Predicate { get; set; }
         public Expression<Func<BpjsClassification, BpjsClassification>> Select { get; set; }

         public List<(Expression<Func<BpjsClassification, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

         public bool IsDescending { get; set; } = false; // default to ascending
         public int PageIndex { get; set; } = 0;
         public int PageSize { get; set; } = 10;
         public bool IsGetAll { get; set; } = false;
         public string SearchTerm { get; set; }
     }

     public class GetSingleBpjsClassificationQuery : IRequest<BpjsClassificationDto>
     {
         public List<Expression<Func<BpjsClassification, object>>> Includes { get; set; }
         public Expression<Func<BpjsClassification, bool>> Predicate { get; set; }
         public Expression<Func<BpjsClassification, BpjsClassification>> Select { get; set; }

         public List<(Expression<Func<BpjsClassification, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

         public bool IsDescending { get; set; } = false; // default to ascending
         public int PageIndex { get; set; } = 0;
         public int PageSize { get; set; } = 10;
         public bool IsGetAll { get; set; } = false;
         public string SearchTerm { get; set; }
     }

     public class ValidateBpjsClassification(Expression<Func<BpjsClassification, bool>>? predicate = null) : IRequest<bool>
     {
         public Expression<Func<BpjsClassification, bool>> Predicate { get; } = predicate!;
     }

     #endregion GET

     #region CREATE

     public class CreateBpjsClassificationRequest(BpjsClassificationDto BpjsClassificationDto) : IRequest<BpjsClassificationDto>
     {
         public BpjsClassificationDto BpjsClassificationDto { get; set; } = BpjsClassificationDto;
     }

     public class BulkValidateBpjsClassification(List<BpjsClassificationDto> BpjsClassificationsToValidate) : IRequest<List<BpjsClassificationDto>>
     {
         public List<BpjsClassificationDto> BpjsClassificationsToValidate { get; } = BpjsClassificationsToValidate;
     }

     public class CreateListBpjsClassificationRequest(List<BpjsClassificationDto> BpjsClassificationDtos) : IRequest<List<BpjsClassificationDto>>
     {
         public List<BpjsClassificationDto> BpjsClassificationDtos { get; set; } = BpjsClassificationDtos;
     }

     #endregion CREATE

     #region Update

     public class UpdateBpjsClassificationRequest(BpjsClassificationDto BpjsClassificationDto) : IRequest<BpjsClassificationDto>
     {
         public BpjsClassificationDto BpjsClassificationDto { get; set; } = BpjsClassificationDto;
     }

     public class UpdateListBpjsClassificationRequest(List<BpjsClassificationDto> BpjsClassificationDtos) : IRequest<List<BpjsClassificationDto>>
     {
         public List<BpjsClassificationDto> BpjsClassificationDtos { get; set; } = BpjsClassificationDtos;
     }

     #endregion Update

     #region DELETE

     public class DeleteBpjsClassificationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
     {
         public long Id { get; set; } = id ?? 0;
         public List<long> Ids { get; set; } = ids ?? [];
     }

     #endregion DELETE
 }

public class BpjsClassificationHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetBpjsClassificationQuery, (List<BpjsClassificationDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleBpjsClassificationQuery, BpjsClassificationDto>, IRequestHandler<ValidateBpjsClassification, bool>,
     IRequestHandler<CreateBpjsClassificationRequest, BpjsClassificationDto>,
     IRequestHandler<BulkValidateBpjsClassification, List<BpjsClassificationDto>>,
     IRequestHandler<CreateListBpjsClassificationRequest, List<BpjsClassificationDto>>,
     IRequestHandler<UpdateBpjsClassificationRequest, BpjsClassificationDto>,
     IRequestHandler<UpdateListBpjsClassificationRequest, List<BpjsClassificationDto>>,
     IRequestHandler<DeleteBpjsClassificationRequest, bool>
 {
     #region GET

     public async Task<List<BpjsClassificationDto>> Handle(BulkValidateBpjsClassification request, CancellationToken cancellationToken)
     {
         var BpjsClassificationDtos = request.BpjsClassificationsToValidate;

         // Ekstrak semua kombinasi yang akan dicari di database
         var BpjsClassificationNames = BpjsClassificationDtos.Select(x => x.Name).Distinct().ToList();
         var a = BpjsClassificationDtos.Select(x => x.CountryId).Distinct().ToList();

         var existingBpjsClassifications = await _unitOfWork.Repository<BpjsClassification>()
             .Entities
             .AsNoTracking()
             .Where(v => BpjsClassificationNames.Contains(v.Name)
                         && a.Contains(v.CountryId))
             .ToListAsync(cancellationToken);

         return existingBpjsClassifications.Adapt<List<BpjsClassificationDto>>();
     }

     public async Task<bool> Handle(ValidateBpjsClassification request, CancellationToken cancellationToken)
     {
         return await _unitOfWork.Repository<BpjsClassification>()
             .Entities
             .AsNoTracking()
             .Where(request.Predicate)  // Apply the Predicate for filtering
             .AnyAsync(cancellationToken);  // Check if any record matches the condition
     }

     public async Task<(List<BpjsClassificationDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetBpjsClassificationQuery request, CancellationToken cancellationToken)
     {
         try
         {
             var query = _unitOfWork.Repository<BpjsClassification>().Entities.AsNoTracking();

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
                         ? ((IOrderedQueryable<BpjsClassification>)query).ThenByDescending(additionalOrderBy.OrderBy)
                         : ((IOrderedQueryable<BpjsClassification>)query).ThenBy(additionalOrderBy.OrderBy);
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
                 query = query.Select(x => new BpjsClassification
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

                 return (pagedItems.Adapt<List<BpjsClassificationDto>>(), request.PageIndex, request.PageSize, totalPages);
             }
             else
             {
                 return ((await query.ToListAsync(cancellationToken)).Adapt<List<BpjsClassificationDto>>(), 0, 1, 1);
             }
         }
         catch (Exception ex)
         {
             // Consider logging the exception
             throw;
         }
     }

     public async Task<BpjsClassificationDto> Handle(GetSingleBpjsClassificationQuery request, CancellationToken cancellationToken)
     {
         try
         {
             var query = _unitOfWork.Repository<BpjsClassification>().Entities.AsNoTracking();

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
                         ? ((IOrderedQueryable<BpjsClassification>)query).ThenByDescending(additionalOrderBy.OrderBy)
                         : ((IOrderedQueryable<BpjsClassification>)query).ThenBy(additionalOrderBy.OrderBy);
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
                 query = query.Select(x => new BpjsClassification
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

             return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<BpjsClassificationDto>();
         }
         catch (Exception ex)
         {
             // Consider logging the exception
             throw;
         }
     }

     #endregion GET

     #region CREATE

     public async Task<BpjsClassificationDto> Handle(CreateBpjsClassificationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDto.Adapt<CreateUpdateBpjsClassificationDto>().Adapt<BpjsClassification>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsClassificationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<BpjsClassificationDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<BpjsClassificationDto>> Handle(CreateListBpjsClassificationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsClassification>().AddAsync(request.BpjsClassificationDtos.Adapt<List<BpjsClassification>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsClassificationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<BpjsClassificationDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion CREATE

     #region UPDATE

     public async Task<BpjsClassificationDto> Handle(UpdateBpjsClassificationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDto.Adapt<BpjsClassificationDto>().Adapt<BpjsClassification>());

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsClassificationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<BpjsClassificationDto>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     public async Task<List<BpjsClassificationDto>> Handle(UpdateListBpjsClassificationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             var result = await _unitOfWork.Repository<BpjsClassification>().UpdateAsync(request.BpjsClassificationDtos.Adapt<List<BpjsClassification>>());
             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsClassificationQuery_"); // Ganti dengan key yang sesuai

             return result.Adapt<List<BpjsClassificationDto>>();
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion UPDATE

     #region DELETE

     public async Task<bool> Handle(DeleteBpjsClassificationRequest request, CancellationToken cancellationToken)
     {
         try
         {
             if (request.Id > 0)
             {
                 await _unitOfWork.Repository<BpjsClassification>().DeleteAsync(request.Id);
             }

             if (request.Ids.Count > 0)
             {
                 await _unitOfWork.Repository<BpjsClassification>().DeleteAsync(x => request.Ids.Contains(x.Id));
             }

             await _unitOfWork.SaveChangesAsync(cancellationToken);

             _cache.Remove("GetBpjsClassificationQuery_"); // Ganti dengan key yang sesuai

             return true;
         }
         catch (Exception)
         {
             throw;
         }
     }

     #endregion DELETE
 }