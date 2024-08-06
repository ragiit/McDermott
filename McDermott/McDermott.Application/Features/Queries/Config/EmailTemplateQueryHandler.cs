using static McDermott.Application.Features.Commands.Config.EmailEmailTemplateCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class EmailEmailTemplateQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetEmailTemplateQuery, List<EmailTemplateDto>>,
        IRequestHandler<CreateEmailTemplateRequest, EmailTemplateDto>,
        IRequestHandler<CreateListEmailTemplateRequest, List<EmailTemplateDto>>,
        IRequestHandler<UpdateEmailTemplateRequest, EmailTemplateDto>,
        IRequestHandler<UpdateListEmailTemplateRequest, List<EmailTemplateDto>>,
        IRequestHandler<DeleteEmailTemplateRequest, bool>
    {
        #region GET

        public async Task<List<EmailTemplateDto>> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetEmailTemplateQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<EmailTemplate>? result))
                {
                    result = await _unitOfWork.Repository<EmailTemplate>().Entities
                        .Include(x => x.By)
                        .Include(x=>x.EmailFrom)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    //return await _unitOfWork.Repository<Counter>().Entities
                    //  .Include(x => x.Physician)
                    //  .Include(x => x.Service)
                    //  .AsNoTracking()
                    //  .Select(Counter => Counter.Adapt<CounterDto>())
                    //  .AsNoTracking()
                    //  .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<EmailTemplateDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<EmailTemplateDto> Handle(CreateEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailTemplate>().AddAsync(request.EmailTemplateDto.Adapt<EmailTemplate>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<EmailTemplateDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmailTemplateDto>> Handle(CreateListEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailTemplate>().AddAsync(request.EmailTemplateDtos.Adapt<List<EmailTemplate>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<EmailTemplateDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<EmailTemplateDto> Handle(UpdateEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailTemplate>().UpdateAsync(request.EmailTemplateDto.Adapt<EmailTemplate>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<EmailTemplateDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmailTemplateDto>> Handle(UpdateListEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<EmailTemplate>().UpdateAsync(request.EmailTemplateDtos.Adapt<List<EmailTemplate>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailTemplateQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<EmailTemplateDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<EmailTemplate>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<EmailTemplate>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetEmailTemplateQuery_"); // Ganti dengan key yang sesuai

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