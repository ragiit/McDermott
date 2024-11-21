using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class MedicamentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMedicamentQuery, (List<MedicamentDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleMedicamentQuery, MedicamentDto>,
        IRequestHandler<ValidateMedicamentQuery, bool>,
        IRequestHandler<BulkValidateMedicamentQuery, List<MedicamentDto>>, IRequestHandler<CreateMedicamentRequest, MedicamentDto>,
        IRequestHandler<CreateListMedicamentRequest, List<MedicamentDto>>,
        IRequestHandler<UpdateMedicamentRequest, MedicamentDto>,
        IRequestHandler<UpdateListMedicamentRequest, List<MedicamentDto>>,
        IRequestHandler<DeleteMedicamentRequest, bool>
    {
        #region GET

        public async Task<MedicamentDto> Handle(GetSingleMedicamentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Medicament>().Entities.AsNoTracking();

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

                // Return the first result as MedicamentDto
                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MedicamentDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<(List<MedicamentDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMedicamentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Medicament>().Entities.AsNoTracking();

                // Apply custom order by if provided
                if (request.OrderBy is not null)
                    query = request.IsDescending ?
                        query.OrderByDescending(request.OrderBy) :
                        query.OrderBy(request.OrderBy);
                //else
                //    query = query.OrderBy(x => x.Name);

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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Medicament
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        FrequencyId = x.FrequencyId,
                        RouteId = x.RouteId,
                        FormId = x.FormId,
                        UomId = x.UomId,
                        ActiveComponentId = x.ActiveComponentId,
                        PregnancyWarning = x.PregnancyWarning,
                        Pharmacologi = x.Pharmacologi,
                        Weather = x.Weather,
                        Food = x.Food,
                        Cronies = x.Cronies,
                        MontlyMax = x.MontlyMax,
                        Dosage = x.Dosage,

                        //Uom = new Uom
                        //{
                        //    Name = x.Uom is null ? string.Empty : x.Uom.Name,
                        //},
                        //Form = new DrugForm
                        //{
                        //    Name = x.Form is null ? string.Empty : x.Form.Name,
                        //},
                        //Frequency = new DrugDosage
                        //{
                        //    Frequency = x.Frequency is null ? string.Empty : x.Frequency.Frequency,
                        //},
                        //Product = new Product
                        //{
                        //    Name = x.Product is null ? string.Empty : x.Product.Name,
                        //},
                        //Route = new DrugRoute
                        //{
                        //    Route = x.Route is null ? string.Empty : x.Route.Route,
                        //},
                    });

                // Paginate and sort
                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                    query,
                    request.PageSize,
                    request.PageIndex,
                    cancellationToken
                );

                return (pagedItems.Adapt<List<MedicamentDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMedicamentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Medicament>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<List<MedicamentDto>> Handle(BulkValidateMedicamentQuery request, CancellationToken cancellationToken)
        {
            var MedicamentDtos = request.MedicamentsToValidate;

            //// Ekstrak semua kombinasi yang akan dicari di database
            //var MedicamentNames = MedicamentDtos.Select(x => x.Name).Distinct().ToList();
            //var MedicamentIds = MedicamentDtos.Select(x => x.MedicamentId).Distinct().ToList();

            //var existingMedicaments = await _unitOfWork.Repository<Medicament>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => MedicamentNames.Contains(v.Name)
            //                && MedicamentIds.Contains(v.MedicamentId))
            //    .ToListAsync(cancellationToken);

            //return existingMedicaments.Adapt<List<MedicamentDto>>();

            return [];
        }

        #endregion GET

        #region CREATE

        public async Task<MedicamentDto> Handle(CreateMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().AddAsync(request.MedicamentDto.Adapt<Medicament>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentDto>> Handle(CreateListMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().AddAsync(request.MedicamentDtos.Adapt<List<Medicament>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MedicamentDto> Handle(UpdateMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().UpdateAsync(request.MedicamentDto.Adapt<Medicament>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MedicamentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicamentDto>> Handle(UpdateListMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Medicament>().UpdateAsync(request.MedicamentDtos.Adapt<List<Medicament>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MedicamentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMedicamentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Medicament>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Medicament>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMedicamentQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetUomQuery_");

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