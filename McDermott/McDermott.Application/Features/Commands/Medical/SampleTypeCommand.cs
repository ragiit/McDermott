namespace McDermott.Application.Features.Commands.Medical
{
    public class SampleTypeCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSampleTypeQuery(Expression<Func<SampleType, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<SampleTypeDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<SampleType, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class BulkValidateSampleTypeQuery(List<SampleTypeDto> SampleTypesToValidate) : IRequest<List<SampleTypeDto>>
        {
            public List<SampleTypeDto> SampleTypesToValidate { get; } = SampleTypesToValidate;
        }

        public class ValidateSampleTypeQuery(Expression<Func<SampleType, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<SampleType, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateSampleTypeRequest(SampleTypeDto SampleTypeDto) : IRequest<SampleTypeDto>
        {
            public SampleTypeDto SampleTypeDto { get; set; } = SampleTypeDto;
        }

        public class CreateListSampleTypeRequest(List<SampleTypeDto> GeneralConsultanCPPTDtos) : IRequest<List<SampleTypeDto>>
        {
            public List<SampleTypeDto> SampleTypeDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateSampleTypeRequest(SampleTypeDto SampleTypeDto) : IRequest<SampleTypeDto>
        {
            public SampleTypeDto SampleTypeDto { get; set; } = SampleTypeDto;
        }

        public class UpdateListSampleTypeRequest(List<SampleTypeDto> SampleTypeDtos) : IRequest<List<SampleTypeDto>>
        {
            public List<SampleTypeDto> SampleTypeDtos { get; set; } = SampleTypeDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteSampleTypeRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}