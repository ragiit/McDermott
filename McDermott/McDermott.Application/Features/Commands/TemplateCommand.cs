namespace McDermott.Application.Features.Commands
{
    public class TemplateCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)
        public class GetTemplateQuery(Expression<Func<Province, bool>>? predicate = null) : IRequest<List<ProvinceDto>>
        {
            public Expression<Func<Province, bool>> Predicate { get; } = predicate!;
        }
        #endregion

        #region CREATE
        public class CreateTemplateRequest(ProvinceDto TemplateDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto TemplateDto { get; set; } = TemplateDto;
        }

        public class CreateListTemplateRequest(List<ProvinceDto> GeneralConsultanCPPTDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> TemplateDtos { get; set; } = GeneralConsultanCPPTDtos;
        }
        #endregion

        #region Update
        public class UpdateTemplateRequest(ProvinceDto TemplateDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto TemplateDto { get; set; } = TemplateDto;
        }

        public class UpdateListTemplateRequest(List<ProvinceDto> TemplateDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> TemplateDtos { get; set; } = TemplateDtos;
        }

        #endregion

        #region DELETE 
        public class DeleteTemplateRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}
