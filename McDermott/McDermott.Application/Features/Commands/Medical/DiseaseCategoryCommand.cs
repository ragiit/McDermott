namespace McDermott.Application.Features.Commands.Medical
{
    public class DiseaseCategoryCommand
    {
        #region GET 

        public class GetDiseaseCategoryQuery(Expression<Func<DiseaseCategory, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DiseaseCategoryDto>>
        {
            public Expression<Func<DiseaseCategory, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto) : IRequest<DiseaseCategoryDto>
        {
            public DiseaseCategoryDto DiseaseCategoryDto { get; set; } = DiseaseCategoryDto;
        }

        public class CreateListDiseaseCategoryRequest(List<DiseaseCategoryDto> DiseaseCategoryDtos) : IRequest<List<DiseaseCategoryDto>>
        {
            public List<DiseaseCategoryDto> DiseaseCategoryDtos { get; set; } = DiseaseCategoryDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto) : IRequest<DiseaseCategoryDto>
        {
            public DiseaseCategoryDto DiseaseCategoryDto { get; set; } = DiseaseCategoryDto;
        }

        public class UpdateListDiseaseCategoryRequest(List<DiseaseCategoryDto> DiseaseCategoryDtos) : IRequest<List<DiseaseCategoryDto>>
        {
            public List<DiseaseCategoryDto> DiseaseCategoryDtos { get; set; } = DiseaseCategoryDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDiseaseCategoryRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}