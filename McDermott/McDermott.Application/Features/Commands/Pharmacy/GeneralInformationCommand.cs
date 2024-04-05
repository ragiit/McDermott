using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacy
{
    public class GeneralInformationCommand
    {
        #region GET 

        public class GetGeneralInformationQuery(Expression<Func<GeneralInformation, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GeneralInformationDto>>
        {
            public Expression<Func<GeneralInformation, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateGeneralInformationRequest(GeneralInformationDto GeneralInformationDto) : IRequest<GeneralInformationDto>
        {
            public GeneralInformationDto GeneralInformationDto { get; set; } = GeneralInformationDto;
        }

        public class CreateListGeneralInformationRequest(List<GeneralInformationDto> GeneralInformationDtos) : IRequest<List<GeneralInformationDto>>
        {
            public List<GeneralInformationDto> GeneralInformationDtos { get; set; } = GeneralInformationDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateGeneralInformationRequest(GeneralInformationDto GeneralInformationDto) : IRequest<GeneralInformationDto>
        {
            public GeneralInformationDto GeneralInformationDto { get; set; } = GeneralInformationDto;
        }

        public class UpdateListGeneralInformationRequest(List<GeneralInformationDto> GeneralInformationDtos) : IRequest<List<GeneralInformationDto>>
        {
            public List<GeneralInformationDto> GeneralInformationDtos { get; set; } = GeneralInformationDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGeneralInformationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}
