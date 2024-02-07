using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Config;
using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public partial class EmailSettingQueryHandler
    {
        internal class GetAllEmailSettingQueryHandler : IRequestHandler<GetEmailSettingQuery, List<EmailSettingDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllEmailSettingQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<EmailSettingDto>> Handle(GetEmailSettingQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<EmailSetting>().Entities
                        .Select(EmailSetting => EmailSetting.Adapt<EmailSettingDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetEmailSettingByIdQueryHandler : IRequestHandler<GetEmailSettingByIdQuery, EmailSettingDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetEmailSettingByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<EmailSettingDto> Handle(GetEmailSettingByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<EmailSetting>().GetByIdAsync(request.Id);

                return result.Adapt<EmailSettingDto>();
            }
        }

        internal class CreateEmailSettingHandler : IRequestHandler<CreateEmailSettingRequest, EmailSettingDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateEmailSettingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<EmailSettingDto> Handle(CreateEmailSettingRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<EmailSetting>().AddAsync(request.EmailSettingDto.Adapt<EmailSetting>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<EmailSettingDto>();
            }
        }

        internal class UpdateEmailSettingHandler : IRequestHandler<UpdateEmailSettingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateEmailSettingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateEmailSettingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<EmailSetting>().UpdateAsync(request.EmailSettingDto.Adapt<EmailSetting>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteEmailSettingHandler : IRequestHandler<DeleteEmailSettingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteEmailSettingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteEmailSettingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<EmailSetting>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListEmailSettingHandler : IRequestHandler<DeleteListEmailSettingRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListEmailSettingHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListEmailSettingRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<EmailSetting>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
