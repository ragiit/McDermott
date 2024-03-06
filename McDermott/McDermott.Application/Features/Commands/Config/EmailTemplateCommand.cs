namespace McDermott.Application.Features.Commands.Config
{
    public class EmailTemplateCommand
    {
        public class GetEmailTemplateQuery : IRequest<List<EmailTemplateDto>>;

        public class GetEmailTemplateByIdQuery : IRequest<EmailTemplateDto>
        {
             public long Id { get; set; }

            public GetEmailTemplateByIdQuery(long id)
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
             public long Id { get; set; }

            public DeleteEmailTemplateRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListEmailTemplateRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListEmailTemplateRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}