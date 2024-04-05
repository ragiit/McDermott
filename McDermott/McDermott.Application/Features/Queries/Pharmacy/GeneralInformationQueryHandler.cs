using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Pharmacy.GeneralInformationCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class GeneralInformationQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralInformationQuery, List<GeneralInformationDto>>,
        IRequestHandler<CreateGeneralInformationRequest, GeneralInformationDto>,
        IRequestHandler<CreateListGeneralInformationRequest, List<GeneralInformationDto>>,
        IRequestHandler<UpdateGeneralInformationRequest, GeneralInformationDto>,
        IRequestHandler<UpdateListGeneralInformationRequest, List<GeneralInformationDto>>,
        IRequestHandler<DeleteGeneralInformationRequest, bool>
    {
        #region GET

        public async Task<List<GeneralInformationDto>> Handle(GetGeneralInformationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralInformationQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralInformation>? result))
                {
                    result = await _unitOfWork.Repository<GeneralInformation>().Entities
                        .Include(x => x.Uom)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralInformationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralInformationDto> Handle(CreateGeneralInformationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralInformation>().AddAsync(request.GeneralInformationDto.Adapt<GeneralInformation>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralInformationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralInformationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralInformationDto>> Handle(CreateListGeneralInformationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralInformation>().AddAsync(request.GeneralInformationDtos.Adapt<List<GeneralInformation>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralInformationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralInformationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<GeneralInformationDto> Handle(UpdateGeneralInformationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralInformation>().UpdateAsync(request.GeneralInformationDto.Adapt<GeneralInformation>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralInformationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralInformationDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralInformationDto>> Handle(UpdateListGeneralInformationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralInformation>().UpdateAsync(request.GeneralInformationDtos.Adapt<List<GeneralInformation>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralInformationQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GeneralInformationDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteGeneralInformationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralInformation>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralInformation>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralInformationQuery_"); // Ganti dengan key yang sesuai
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
