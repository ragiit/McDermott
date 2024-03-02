using static McDermott.Application.Features.Commands.Config.EmailTemplateCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class EmailTemplateQueryHandler
    {
        internal class GetAllEmailTemplateQueryHandler : IRequestHandler<GetEmailTemplateQuery, List<EmailTemplateDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllEmailTemplateQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<EmailTemplateDto>> Handle(GetEmailTemplateQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<EmailTemplate>().Entities
                        .Include(x => x.By)
                        .Include(x => x.ToPartner)
                        .AsNoTracking()
                        .Select(EmailTemplate => EmailTemplate.Adapt<EmailTemplateDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetEmailTemplateByIdQueryHandler : IRequestHandler<GetEmailTemplateByIdQuery, EmailTemplateDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetEmailTemplateByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<EmailTemplateDto> Handle(GetEmailTemplateByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<EmailTemplate>().GetByIdAsync(request.Id);

                return result.Adapt<EmailTemplateDto>();
            }
        }

        internal class CreateEmailTemplateHandler : IRequestHandler<CreateEmailTemplateRequest, EmailTemplateDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateEmailTemplateHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<EmailTemplateDto> Handle(CreateEmailTemplateRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<EmailTemplate>().AddAsync(request.EmailTemplateDto.Adapt<EmailTemplate>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<EmailTemplateDto>();
            }
        }

        internal class UpdateEmailTemplateHandler : IRequestHandler<UpdateEmailTemplateRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateEmailTemplateHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateEmailTemplateRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<EmailTemplate>().UpdateAsync(request.EmailTemplateDto.Adapt<EmailTemplate>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteEmailTemplateHandler : IRequestHandler<DeleteEmailTemplateRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteEmailTemplateHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteEmailTemplateRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<EmailTemplate>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListEmailTemplateHandler : IRequestHandler<DeleteListEmailTemplateRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListEmailTemplateHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListEmailTemplateRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<EmailTemplate>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}