namespace McDermott.Application.Features.Commands.Medical
{
    public class SampleTypeCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSampleTypeQuery(Expression<Func<SampleType, bool>>? predicate = null, bool removeCache = false) : IRequest<List<SampleTypeDto>>
        {
            public Expression<Func<SampleType, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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
