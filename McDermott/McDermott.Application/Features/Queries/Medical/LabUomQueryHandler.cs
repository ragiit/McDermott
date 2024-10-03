using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.LabUomCommand;

using static McDermott.Application.Features.Commands.Medical.LabUomCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class LabUomQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetLabUomQuery, (List<LabUomDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateLabUomRequest, LabUomDto>,
        IRequestHandler<BulkValidateLabUomQuery, List<LabUomDto>>,
        IRequestHandler<CreateListLabUomRequest, List<LabUomDto>>,
        IRequestHandler<UpdateLabUomRequest, LabUomDto>,
        IRequestHandler<UpdateListLabUomRequest, List<LabUomDto>>,
        IRequestHandler<DeleteLabUomRequest, bool>
    {
        #region GET

        public async Task<List<LabUomDto>> Handle(BulkValidateLabUomQuery request, CancellationToken cancellationToken)
        {
            var LabUomDtos = request.LabUomsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var LabUomNames = LabUomDtos.Select(x => x.Name).Distinct().ToList();
            var Codes = LabUomDtos.Select(x => x.Code).Distinct().ToList();

            var existingLabUoms = await _unitOfWork.Repository<LabUom>()
                .Entities
                .AsNoTracking()
                .Where(v => LabUomNames.Contains(v.Name)
                            && Codes.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingLabUoms.Adapt<List<LabUomDto>>();
        }

        public async Task<(List<LabUomDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetLabUomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<LabUom>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                        );
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

                return (pagedItems.Adapt<List<LabUomDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateLabUomQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<LabUom>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<LabUomDto> Handle(CreateLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().AddAsync(request.LabUomDto.Adapt<LabUom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabUomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabUomDto>> Handle(CreateListLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().AddAsync(request.LabUomDtos.Adapt<List<LabUom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabUomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LabUomDto> Handle(UpdateLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().UpdateAsync(request.LabUomDto.Adapt<LabUom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<LabUomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LabUomDto>> Handle(UpdateListLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<LabUom>().UpdateAsync(request.LabUomDtos.Adapt<List<LabUom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<LabUomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLabUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<LabUom>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<LabUom>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetLabUomQuery_"); // Ganti dengan key yang sesuai

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