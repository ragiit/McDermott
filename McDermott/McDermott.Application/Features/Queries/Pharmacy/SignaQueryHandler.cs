using McDermott.Application.Dtos.Pharmacy;
using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Pharmacy.SignaCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    //public class SignaQueryHandler
    //{
    public class SignaQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
    IRequestHandler<GetSignaQuery, List<SignaDto>>,
    IRequestHandler<CreateSignaRequest, SignaDto>,
    IRequestHandler<UpdateSignaRequest, SignaDto>,
    IRequestHandler<UpdateListSignaRequest, List<SignaDto>>,
    IRequestHandler<DeleteSignaRequest, bool>
    {
        #region GET

        public async Task<List<SignaDto>> Handle(GetSignaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetSignaQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique 

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Signa>? result))
                {
                    result = await _unitOfWork.Repository<Signa>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result?.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<SignaDto>>();
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

