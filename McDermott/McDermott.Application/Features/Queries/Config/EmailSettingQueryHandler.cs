using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public partial class EmailSettingQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetEmailSettingQuery, List<EmailSettingDto>>,
        IRequestHandler<CreateEmailSettingRequest, EmailSettingDto>,
        IRequestHandler<CreateListEmailSettingRequest, List<EmailSettingDto>>,
        IRequestHandler<UpdateEmailSettingRequest, EmailSettingDto>,
        IRequestHandler<UpdateListEmailSettingRequest, List<EmailSettingDto>>,
        IRequestHandler<DeleteEmailSettingRequest, bool>
    {
        #region GET

        public async Task<List<EmailSettingDto>> Handle(GetEmailSettingQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetEmailSettingQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<EmailSetting>? result))
                {
                    result = await _unitOfWork.Repository<EmailSetting>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<EmailSettingDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<EmailSettingDto> Handle(CreateEmailSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailSetting>().AddAsync(request.EmailSettingDto.Adapt<EmailSetting>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailSettingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<EmailSettingDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmailSettingDto>> Handle(CreateListEmailSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailSetting>().AddAsync(request.EmailSettingDtos.Adapt<List<EmailSetting>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailSettingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<EmailSettingDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<EmailSettingDto> Handle(UpdateEmailSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailSetting>().UpdateAsync(request.EmailSettingDto.Adapt<EmailSetting>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailSettingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<EmailSettingDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmailSettingDto>> Handle(UpdateListEmailSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailSetting>().UpdateAsync(request.EmailSettingDtos.Adapt<List<EmailSetting>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailSettingQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<EmailSettingDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteEmailSettingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<EmailSetting>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<EmailSetting>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailSettingQuery_"); // Ganti dengan key yang sesuai

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