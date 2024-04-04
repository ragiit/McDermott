namespace McDermott.Application.Features.Commands
{
    public class TemplateCommand
    {
        #region GET 

        public class GetTemplateQuery(Expression<Func<Province, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ProvinceDto>>
        {
            public Expression<Func<Province, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateTemplateRequest(ProvinceDto TemplateDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto TemplateDto { get; set; } = TemplateDto;
        }

        public class CreateListTemplateRequest(List<ProvinceDto> TemplateDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> TemplateDtos { get; set; } = TemplateDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateTemplateRequest(ProvinceDto TemplateDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto TemplateDto { get; set; } = TemplateDto;
        }

        public class UpdateListTemplateRequest(List<ProvinceDto> TemplateDtos) : IRequest<List<ProvinceDto>>
        {
            public List<ProvinceDto> TemplateDtos { get; set; } = TemplateDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteTemplateRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}