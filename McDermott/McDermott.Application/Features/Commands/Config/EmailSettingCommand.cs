namespace McDermott.Application.Features.Commands.Config
{
    public partial class EmailSettingCommand
    {
        public class GetEmailSettingQuery : IRequest<List<EmailSettingDto>>;

        public class GetEmailSettingByIdQuery : IRequest<EmailSettingDto>
        {
             public long Id { get; set; }

            public GetEmailSettingByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateEmailSettingRequest : IRequest<EmailSettingDto>
        {
            public EmailSettingDto EmailSettingDto { get; set; }

            public CreateEmailSettingRequest(EmailSettingDto EmailSettingDto)
            {
                this.EmailSettingDto = EmailSettingDto;
            }
        }

        public class UpdateEmailSettingRequest : IRequest<bool>
        {
            public EmailSettingDto EmailSettingDto { get; set; }

            public UpdateEmailSettingRequest(EmailSettingDto EmailSettingDto)
            {
                this.EmailSettingDto = EmailSettingDto;
            }
        }

        public class DeleteEmailSettingRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteEmailSettingRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListEmailSettingRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListEmailSettingRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}