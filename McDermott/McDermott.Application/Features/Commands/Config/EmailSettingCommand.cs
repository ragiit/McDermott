using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public partial class EmailSettingCommand
    {
        public class GetEmailSettingQuery : IRequest<List<EmailSettingDto>>;

        public class GetEmailSettingByIdQuery : IRequest<EmailSettingDto>
        {
            public int Id { get; set; }

            public GetEmailSettingByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteEmailSettingRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListEmailSettingRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListEmailSettingRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
