using McDermott.Application.Dtos.Pharmacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class ConcoctionCommand
    {
        #region GET Concoction

        public class GetAllConcoctionQuery(Expression<Func<Concoction, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ConcoctionDto>>
        {
            public Expression<Func<Concoction, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSingleConcoctionQuery : IRequest<ConcoctionDto>
        {
            public List<Expression<Func<Concoction, object>>> Includes { get; set; }
            public Expression<Func<Concoction, bool>> Predicate { get; set; }
            public Expression<Func<Concoction, Concoction>> Select { get; set; }

            public List<(Expression<Func<Concoction, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetConcoctionQuery : IRequest<(List<ConcoctionDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Concoction, object>>> Includes { get; set; }
            public Expression<Func<Concoction, bool>> Predicate { get; set; }
            public Expression<Func<Concoction, Concoction>> Select { get; set; }

            public List<(Expression<Func<Concoction, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateConcoctionQuery(List<ConcoctionDto> ConcoctionToValidate) : IRequest<List<ConcoctionDto>>
        {
            public List<ConcoctionDto> ConcoctionToValidate { get; } = ConcoctionToValidate;
        }

        public class ValidateConcoctionQuery(Expression<Func<Concoction, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Concoction, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

        #region CREATE

        public class CreateConcoctionRequest(ConcoctionDto ConcoctionDto) : IRequest<ConcoctionDto>
        {
            public ConcoctionDto ConcoctionDto { get; set; } = ConcoctionDto;
        }

        public class CreateListConcoctionRequest(List<ConcoctionDto> ConcoctionDtos) : IRequest<List<ConcoctionDto>>
        {
            public List<ConcoctionDto> ConcoctionDtos { get; set; } = ConcoctionDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateConcoctionRequest(ConcoctionDto ConcoctionDto) : IRequest<ConcoctionDto>
        {
            public ConcoctionDto ConcoctionDto { get; set; } = ConcoctionDto;
        }

        public class UpdateListConcoctionRequest(List<ConcoctionDto> ConcoctionDtos) : IRequest<List<ConcoctionDto>>
        {
            public List<ConcoctionDto> ConcoctionDtos { get; set; } = ConcoctionDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteConcoctionRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}