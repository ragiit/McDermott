

using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class AccidentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAccidentQuery, List<AccidentDto>>,
        IRequestHandler<CreateAccidentRequest, AccidentDto>,
        IRequestHandler<CreateListAccidentRequest, List<AccidentDto>>,
        IRequestHandler<UpdateAccidentRequest, AccidentDto>,
        IRequestHandler<UpdateListAccidentRequest, List<AccidentDto>>,
        IRequestHandler<DeleteAccidentRequest, bool>
    {
        #region GET

        public async Task<List<AccidentDto>> Handle(GetAccidentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAccidentQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Accident>? result))
                {
                    result = await _unitOfWork.Repository<Accident>().Entities
                       .Include(x => x.Department)
                       .Include(x => x.Employee)
                       .Include(x => x.SafetyPersonnel)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);


                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<AccidentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<AccidentDto> Handle(CreateAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().AddAsync(request.AccidentDto.Adapt<Accident>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<AccidentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AccidentDto>> Handle(CreateListAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().AddAsync(request.AccidentDtos.Adapt<List<Accident>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<AccidentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<AccidentDto> Handle(UpdateAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().UpdateAsync(request.AccidentDto.Adapt<Accident>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<AccidentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AccidentDto>> Handle(UpdateListAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Accident>().UpdateAsync(request.AccidentDtos.Adapt<List<Accident>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<AccidentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteAccidentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Accident>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Accident>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAccidentQuery_"); // Ganti dengan key yang sesuai

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