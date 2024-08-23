using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class GeneralConsultanServiceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGeneralConsultanServiceQuery, List<GeneralConsultanServiceDto>>,
        IRequestHandler<CreateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<CreateListGeneralConsultanServiceRequest, List<GeneralConsultanServiceDto>>,
        IRequestHandler<UpdateGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<UpdateStatusGeneralConsultanServiceRequest, GeneralConsultanServiceDto>,
        IRequestHandler<DeleteGeneralConsultanServiceRequest, bool>,
        IRequestHandler<GetGeneralConsultationLogQuery, List<GeneralConsultanlogDto>>,
        IRequestHandler<CreateGeneralConsultationLogRequest, GeneralConsultanlogDto>,
        IRequestHandler<CreateListGeneralConsultationLogRequest, List<GeneralConsultanlogDto>>,
        IRequestHandler<UpdateGeneralConsultationLogRequest, GeneralConsultanlogDto>,
        IRequestHandler<DeleteGeneralConsultationLogRequest, bool>,
        IRequestHandler<GetGeneralConsultanServiceCountQuery, int>
    {
        #region GET

        public async Task<int> Handle(GetGeneralConsultanServiceCountQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GeneralConsultanService>().Entities
                .Where(request.Predicate)
                .Include(x => x.Service)
                .AsNoTracking()
                .CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<List<GeneralConsultanServiceDto>> Handle(GetGeneralConsultanServiceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultanServiceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultanService>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                        .Include(z => z.Service)
                        .Include(z => z.Pratitioner)
                        .Include(z => z.ClassType)
                        .Include(z => z.InsurancePolicy)
                        .Include(z => z.Patient)
                        .Include(z => z.Patient!.Gender)
                        .Include(z => z.Patient.Department)
                        .Include(z => z.Patient.IdCardCountry)
                        //.ThenInclude(z => z.Gender)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<GeneralConsultanServiceDto> Handle(CreateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDto.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(req.Adapt<GeneralConsultanService>());

                var currentDate = DateTime.Now;
                string year = currentDate.ToString("yyyy");
                string month = currentDate.ToString("MM");

                var lastTransaction = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                    .Where(x => x.CreatedDate.Value.Year == currentDate.Year
                && x.CreatedDate.Value.Month == currentDate.Month).OrderByDescending(x => x.Id).Select(x => x.Reference).FirstOrDefaultAsync();

                int newSequence = 1;

                if (lastTransaction != null)
                {
                    string lastSequenceNumber = lastTransaction;

                    // Pastikan lastSequenceNumber memiliki format yang benar
                    if (lastSequenceNumber.Length == 16 && lastSequenceNumber.StartsWith("GC/"))
                    {
                        // Extract the sequence part (XXXXX) from the last sequence number
                        string sequencePart = lastSequenceNumber.Substring(11, 5);

                        if (int.TryParse(sequencePart, out int lastSequence))
                        {
                            // Increment sequence number
                            newSequence = lastSequence + 1;
                        }
                        else
                        {
                            // Handle the case where the sequence part is not a valid number
                            throw new FormatException($"The sequence part '{sequencePart}' is not a valid number.");
                        }
                    }
                    else
                    {
                        // Handle the case where lastSequenceNumber does not match expected format
                        throw new FormatException($"The last sequence number '{lastSequenceNumber}' does not match the expected format 'GC/YYYY/MM/XXXXX'.");
                    }
                }

                // Generate the new sequence number with format GC/YYYY/MM/XXXXX
                string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

                result.Reference = sequenceNumber;

                // Generate the new sequence number with format GC/YYYY/MM/XXXXX
                //string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanServiceDto>> Handle(CreateListGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDtos.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().AddAsync(req.Adapt<List<GeneralConsultanService>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_");

                return result.Adapt<List<GeneralConsultanServiceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region Update

        public async Task<GeneralConsultanServiceDto> Handle(UpdateStatusGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultanService>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id) ?? new();

                result.Status = request.Status;

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GenerateGCSequenceNumberAsync(List<GeneralConsultanServiceDto> generalConsultanServices)
        {
            var currentDate = DateTime.Now;
            string year = currentDate.ToString("yyyy");
            string month = currentDate.ToString("MM");

            // Ambil transaksi terakhir di bulan dan tahun ini
            //var lastTransaction = await _context.Transactions
            //    .Where(t => t.ConfirmDate.Year == currentDate.Year && t.ConfirmDate.Month == currentDate.Month)
            //    .OrderByDescending(t => t.Id)
            //    .FirstOrDefaultAsync();

            var lastTransaction = generalConsultanServices.Where(x => x.CreatedDate.GetValueOrDefault().Year == currentDate.Year
            && x.CreatedDate.GetValueOrDefault().Month == currentDate.Month).OrderByDescending(x => x.Id).FirstOrDefault();

            int newSequence = 1;

            if (lastTransaction != null)
            {
                // Extract the sequence part (XXXXX) from the last sequence number
                string lastSequenceNumber = lastTransaction.Reference;
                int lastSequence = int.Parse(lastSequenceNumber.Substring(10, 5));

                // Increment sequence number
                newSequence = lastSequence + 1;
            }

            // Generate the new sequence number with format GC/YYYY/MM/XXXXX
            string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

            return sequenceNumber;
        }

        public async Task<GeneralConsultanServiceDto> Handle(UpdateGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var req = request.GeneralConsultanServiceDto.Adapt<CreateUpdateGeneralConsultanServiceDto>();
                var result = await _unitOfWork.Repository<GeneralConsultanService>().UpdateAsync(req.Adapt<GeneralConsultanService>());

                if (string.IsNullOrWhiteSpace(result.Reference))
                {
                    var currentDate = DateTime.Now;
                    string year = currentDate.ToString("yyyy");
                    string month = currentDate.ToString("MM");

                    var lastTransaction = await _unitOfWork.Repository<GeneralConsultanService>().Entities
                        .Where(x => x.CreatedDate.Value.Year == currentDate.Year
                    && x.CreatedDate.Value.Month == currentDate.Month).OrderByDescending(x => x.Id).Select(x => x.Reference).FirstOrDefaultAsync();

                    int newSequence = 1;

                    if (lastTransaction != null)
                    {
                        string lastSequenceNumber = lastTransaction;

                        // Pastikan lastSequenceNumber memiliki format yang benar
                        if (lastSequenceNumber.Length == 16 && lastSequenceNumber.StartsWith("GC/"))
                        {
                            // Extract the sequence part (XXXXX) from the last sequence number
                            string sequencePart = lastSequenceNumber.Substring(11, 5);

                            if (int.TryParse(sequencePart, out int lastSequence))
                            {
                                // Increment sequence number
                                newSequence = lastSequence + 1;
                            }
                            else
                            {
                                // Handle the case where the sequence part is not a valid number
                                throw new FormatException($"The sequence part '{sequencePart}' is not a valid number.");
                            }
                        }
                        else
                        {
                            // Handle the case where lastSequenceNumber does not match expected format
                            throw new FormatException($"The last sequence number '{lastSequenceNumber}' does not match the expected format 'GC/YYYY/MM/XXXXX'.");
                        }
                    }

                    // Generate the new sequence number with format GC/YYYY/MM/XXXXX
                    string sequenceNumber = $"GC/{year}/{month}/{newSequence:D5}";

                    result.Reference = sequenceNumber;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanServiceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update

        #region Delete

        public async Task<bool> Handle(DeleteGeneralConsultanServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultanService>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultanServiceQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete

        #region GET General Consultan Service Log

        public async Task<List<GeneralConsultanlogDto>> Handle(GetGeneralConsultationLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetGeneralConsultationLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GeneralConsultationLog>? result))
                {
                    result = await _unitOfWork.Repository<GeneralConsultationLog>().Entities
                        .Include(z => z.GeneralConsultanService)
                        .Include(z => z.ProcedureRoom)
                        .Include(z => z.UserBy)

                        //.ThenInclude(z => z.Gender)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GeneralConsultanlogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET General Consultan Service Log

        #region CREATE General Consultan Service Log

        public async Task<GeneralConsultanlogDto> Handle(CreateGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationLog>().AddAsync(request.GeneralConsultanlogDto.Adapt<GeneralConsultationLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_");

                return result.Adapt<GeneralConsultanlogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GeneralConsultanlogDto>> Handle(CreateListGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationLog>().AddAsync(request.GeneralConsultanlogDto.Adapt<List<GeneralConsultationLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_");

                return result.Adapt<List<GeneralConsultanlogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE General Consultan Service Log

        #region Update General Consultan Service Log

        public async Task<GeneralConsultanlogDto> Handle(UpdateGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GeneralConsultationLog>().UpdateAsync(request.GeneralConsultanlogDto.Adapt<GeneralConsultationLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GeneralConsultanlogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Update General Consultan Service Log

        #region Delete General Consultan Service Log

        public async Task<bool> Handle(DeleteGeneralConsultationLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultationLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GeneralConsultationLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGeneralConsultationLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Delete General Consultan Service Log
    }
}