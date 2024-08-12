namespace McDermott.Application.Features.Commands.Config
{
    public sealed class EmailEmailTemplateCommand
    {
        #region GET 

        public class GetEmailTemplateQuery(Expression<Func<EmailTemplate, bool>>? predicate = null, bool removeCache = false) : IRequest<List<EmailTemplateDto>>
        {
            public Expression<Func<EmailTemplate, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateEmailTemplateRequest(EmailTemplateDto EmailTemplateDto) : IRequest<EmailTemplateDto>
        {
            public EmailTemplateDto EmailTemplateDto { get; set; } = EmailTemplateDto;
        }

        public class CreateListEmailTemplateRequest(List<EmailTemplateDto> EmailTemplateDtos) : IRequest<List<EmailTemplateDto>>
        {
            public List<EmailTemplateDto> EmailTemplateDtos { get; set; } = EmailTemplateDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateEmailTemplateRequest(EmailTemplateDto EmailTemplateDto) : IRequest<EmailTemplateDto>
        {
            public EmailTemplateDto EmailTemplateDto { get; set; } = EmailTemplateDto;
        }

        public class UpdateListEmailTemplateRequest(List<EmailTemplateDto> EmailTemplateDtos) : IRequest<List<EmailTemplateDto>>
        {
            public List<EmailTemplateDto> EmailTemplateDtos { get; set; } = EmailTemplateDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteEmailTemplateRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}