using McDermott.Application.Dtos.Pharmacies;
using McDermott.Application.Dtos.Queue;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacies.SignaCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    //public class SignaQueryHandler
    //{
    public class SignaQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetSignaQuery, (List<SignaDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateSignaRequest, SignaDto>,
        IRequestHandler<CreateListSignaRequest, List<SignaDto>>,
        IRequestHandler<UpdateSignaRequest, SignaDto>,
        IRequestHandler<UpdateListSignaRequest, List<SignaDto>>,
        IRequestHandler<ValidateSignaQuery, bool>,
        IRequestHandler<BulkValidateSignaQuery, List<SignaDto>>,
        IRequestHandler<DeleteSignaRequest, bool>
    {
        #region GET

        public async Task<bool> Handle(ValidateSignaQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Signa>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<List<SignaDto>> Handle(BulkValidateSignaQuery request, CancellationToken cancellationToken)
        {
            var SignaDtos = request.SignasToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var SignaNames = SignaDtos.Select(x => x.Name).Distinct().ToList();

            var existingSignas = await _unitOfWork.Repository<Signa>()
                .Entities
                .AsNoTracking()
                .Where(v => SignaNames.Contains(v.Name))
                .ToListAsync(cancellationToken);

            return existingSignas.Adapt<List<SignaDto>>();
        }

        public async Task<(List<SignaDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetSignaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Signa>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
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

                return (pagedItems.Adapt<List<SignaDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<SignaDto> Handle(CreateSignaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Signa>().AddAsync(request.SignaDto.Adapt<Signa>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSignaQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SignaDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SignaDto>> Handle(CreateListSignaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Signa>().AddAsync(request.SignaDtos.Adapt<List<Signa>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSignaQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SignaDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SignaDto> Handle(UpdateSignaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Signa>().UpdateAsync(request.SignaDto.Adapt<Signa>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSignaQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<SignaDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SignaDto>> Handle(UpdateListSignaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Signa>().UpdateAsync(request.SignaDtos.Adapt<List<Signa>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSignaQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<SignaDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSignaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Signa>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Signa>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetSignaQuery_"); // Ganti dengan key yang sesuai

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