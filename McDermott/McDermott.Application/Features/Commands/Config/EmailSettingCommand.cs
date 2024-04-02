namespace McDermott.Application.Features.Commands.Config
{
    public partial class EmailSettingCommand
    {
        #region GET 

        public class GetEmailSettingQuery(Expression<Func<EmailSetting, bool>>? predicate = null, bool removeCache = false) : IRequest<List<EmailSettingDto>>
        {
            public Expression<Func<EmailSetting, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateEmailSettingRequest(EmailSettingDto EmailSettingDto) : IRequest<EmailSettingDto>
        {
            public EmailSettingDto EmailSettingDto { get; set; } = EmailSettingDto;
        }

        public class CreateListEmailSettingRequest(List<EmailSettingDto> EmailSettingDtos) : IRequest<List<EmailSettingDto>>
        {
            public List<EmailSettingDto> EmailSettingDtos { get; set; } = EmailSettingDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateEmailSettingRequest(EmailSettingDto EmailSettingDto) : IRequest<EmailSettingDto>
        {
            public EmailSettingDto EmailSettingDto { get; set; } = EmailSettingDto;
        }

        public class UpdateListEmailSettingRequest(List<EmailSettingDto> EmailSettingDtos) : IRequest<List<EmailSettingDto>>
        {
            public List<EmailSettingDto> EmailSettingDtos { get; set; } = EmailSettingDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteEmailSettingRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE 
    }
}