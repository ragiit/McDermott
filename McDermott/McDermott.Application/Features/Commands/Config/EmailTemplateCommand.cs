using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class EmailTemplateCommand
    {
        public class GetEmailTemplateQuery : IRequest<List<EmailTemplateDto>>;

        public class GetEmailTemplateByIdQuery : IRequest<EmailTemplateDto>
        {
            public int Id { get; set; }

            public GetEmailTemplateByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateEmailTemplateRequest : IRequest<EmailTemplateDto>
        {
            public EmailTemplateDto EmailTemplateDto { get; set; }

            public CreateEmailTemplateRequest(EmailTemplateDto EmailTemplateDto)
            {
                this.EmailTemplateDto = EmailTemplateDto;
            }
        }

        public class UpdateEmailTemplateRequest : IRequest<bool>
        {
            public EmailTemplateDto EmailTemplateDto { get; set; }

            public UpdateEmailTemplateRequest(EmailTemplateDto EmailTemplateDto)
            {
                this.EmailTemplateDto = EmailTemplateDto;
            }
        }

        public class DeleteEmailTemplateRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteEmailTemplateRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListEmailTemplateRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListEmailTemplateRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
