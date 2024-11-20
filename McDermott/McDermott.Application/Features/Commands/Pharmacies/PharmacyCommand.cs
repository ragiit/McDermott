
using McDermott.Application.Dtos.Pharmacies;

namespace McDermott.Application.Features.Commands.Pharmacies
{
    public class PharmacyCommand
    {
        #region Pharmacy
        #region GET Pharmacy

        public class GetAllPharmacyQuery(Expression<Func<Pharmacy, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PharmacyDto>>
        {
            public Expression<Func<Pharmacy, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSinglePharmacyQuery : IRequest<PharmacyDto>
        {
            public List<Expression<Func<Pharmacy, object>>> Includes { get; set; }
            public Expression<Func<Pharmacy, bool>> Predicate { get; set; }
            public Expression<Func<Pharmacy, Pharmacy>> Select { get; set; }

            public List<(Expression<Func<Pharmacy, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetPharmacyQuery : IRequest<(List<PharmacyDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Pharmacy, object>>> Includes { get; set; }
            public Expression<Func<Pharmacy, bool>> Predicate { get; set; }
            public Expression<Func<Pharmacy, Pharmacy>> Select { get; set; }

            public List<(Expression<Func<Pharmacy, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidatePharmacyQuery(List<PharmacyDto> PharmacyToValidate) : IRequest<List<PharmacyDto>>
        {
            public List<PharmacyDto> PharmacyToValidate { get; } = PharmacyToValidate;
        }

        public class ValidatePharmacyQuery(Expression<Func<Pharmacy, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Pharmacy, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

        #region CREATE

        public class CreatePharmacyRequest(PharmacyDto PharmacyDto) : IRequest<PharmacyDto>
        {
            public PharmacyDto PharmacyDto { get; set; } = PharmacyDto;
        }

        public class CreateListPharmacyRequest(List<PharmacyDto> PharmacyDtos) : IRequest<List<PharmacyDto>>
        {
            public List<PharmacyDto> PharmacyDtos { get; set; } = PharmacyDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdatePharmacyRequest(PharmacyDto PharmacyDto) : IRequest<PharmacyDto>
        {
            public PharmacyDto PharmacyDto { get; set; } = PharmacyDto;
        }

        public class UpdateListPharmacyRequest(List<PharmacyDto> PharmacyDtos) : IRequest<List<PharmacyDto>>
        {
            public List<PharmacyDto> PharmacyDtos { get; set; } = PharmacyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeletePharmacyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
        #endregion

        #region PharmacyLog
        #region Get
        #region GET Pharmacy

        public class GetAllPharmacyLogQuery(Expression<Func<PharmacyLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PharmacyLogDto>>
        {
            public Expression<Func<PharmacyLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }
        public class GetSinglePharmacyLogQuery : IRequest<PharmacyLogDto>
        {
            public List<Expression<Func<PharmacyLog, object>>> Includes { get; set; }
            public Expression<Func<PharmacyLog, bool>> Predicate { get; set; }
            public Expression<Func<PharmacyLog, PharmacyLog>> Select { get; set; }

            public List<(Expression<Func<PharmacyLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetPharmacyLogQuery : IRequest<(List<PharmacyLogDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<PharmacyLog, object>>> Includes { get; set; }
            public Expression<Func<PharmacyLog, bool>> Predicate { get; set; }
            public Expression<Func<PharmacyLog, PharmacyLog>> Select { get; set; }

            public List<(Expression<Func<PharmacyLog, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidatePharmacyLogQuery(List<PharmacyLogDto> PharmacyLogToValidate) : IRequest<List<PharmacyLogDto>>
        {
            public List<PharmacyLogDto> PharmacyLogToValidate { get; } = PharmacyLogToValidate;
        }

        public class ValidatePharmacyLogQuery(Expression<Func<PharmacyLog, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<PharmacyLog, bool>> Predicate { get; } = predicate!;
        }
        #endregion GET Education Program Detail

        #endregion

        #region Create
        public class CreatePharmacyLogRequest(PharmacyLogDto PharmacyLogDto) : IRequest<PharmacyLogDto>
        {
            public PharmacyLogDto PharmacyLogDto { get; set; } = PharmacyLogDto;
        }

        public class CreateListPharmacyLogRequest(List<PharmacyLogDto> PharmacyLogDtos) : IRequest<List<PharmacyLogDto>>
        {
            public List<PharmacyLogDto> PharmacyLogDtos { get; set; } = PharmacyLogDtos;
        }

        #endregion

        #region Update
        public class UpdatePharmacyLogRequest(PharmacyLogDto PharmacyLogDto) : IRequest<PharmacyLogDto>
        {
            public PharmacyLogDto PharmacyLogDto { get; set; } = PharmacyLogDto;
        }

        public class UpdateListPharmacyLogRequest(List<PharmacyLogDto> PharmacyLogDtos) : IRequest<List<PharmacyLogDto>>
        {
            public List<PharmacyLogDto> PharmacyLogDtos { get; set; } = PharmacyLogDtos;
        }
        #endregion

        #region Delete
        public class DeletePharmacyLogRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion

        #endregion
    }
}
