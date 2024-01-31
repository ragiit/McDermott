using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.DiagnosisCommand;

namespace McDermott.Application.Features.Queries
{
    public class DiagnosisQueryHandler
    {
        internal class GetAllDiagnosisQueryHandler : IRequestHandler<GetDiagnosisQuery, List<DiagnosisDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllDiagnosisQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DiagnosisDto>> Handle(GetDiagnosisQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    return await _unitOfWork.Repository<Diagnosis>().Entities
                        .Include(x => x.DiseaseCategory)
                        .Include(x => x.CronisKategory)
                        .Select(Diagnosis => Diagnosis.Adapt<DiagnosisDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                }
                catch (Exception ee)
                {
                    throw;
                }
            }
        }

        internal class GetDiagnosisByIdQueryHandler : IRequestHandler<GetDiagnosisByIdQuery, DiagnosisDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDiagnosisByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DiagnosisDto> Handle(GetDiagnosisByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Diagnosis>().GetByIdAsync(request.Id);

                return result.Adapt<DiagnosisDto>();
            }
        }

        internal class CreateDiagnosisHandler : IRequestHandler<CreateDiagnosisRequest, DiagnosisDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDiagnosisHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DiagnosisDto> Handle(CreateDiagnosisRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Diagnosis>().AddAsync(request.DiagnosisDto.Adapt<Diagnosis>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<DiagnosisDto>();
            }
        }

        internal class UpdateDiagnosisHandler : IRequestHandler<UpdateDiagnosisRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDiagnosisHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateDiagnosisRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Diagnosis>().UpdateAsync(request.DiagnosisDto.Adapt<Diagnosis>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteDiagnosisHandler : IRequestHandler<DeleteDiagnosisRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDiagnosisHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDiagnosisRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Diagnosis>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListDiagnosisHandler : IRequestHandler<DeleteListDiagnosisRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDiagnosisHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDiagnosisRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Diagnosis>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}