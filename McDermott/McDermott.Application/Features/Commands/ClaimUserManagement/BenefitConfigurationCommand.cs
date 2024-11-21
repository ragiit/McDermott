using McDermott.Application.Dtos.ClaimUserManagement;

namespace McDermott.Application.Features.Commands.ClaimUserManagement
{
    public class BenefitConfigurationCommand
    {
        #region GET Education Program Detail

        public class GetAllBenefitConfigurationQuery(Expression<Func<BenefitConfiguration, bool>>? predicate = null, bool removeCache = false) : IRequest<List<BenefitConfigurationDto>>
        {
            public Expression<Func<BenefitConfiguration, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetSingleBenefitConfigurationQuery : IRequest<BenefitConfigurationDto>
        {
            public List<Expression<Func<BenefitConfiguration, object>>> Includes { get; set; }
            public Expression<Func<BenefitConfiguration, bool>> Predicate { get; set; }
            public Expression<Func<BenefitConfiguration, BenefitConfiguration>> Select { get; set; }

            public List<(Expression<Func<BenefitConfiguration, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetBenefitConfigurationQuery : IRequest<(List<BenefitConfigurationDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<BenefitConfiguration, object>>> Includes { get; set; }
            public Expression<Func<BenefitConfiguration, bool>> Predicate { get; set; }
            public Expression<Func<BenefitConfiguration, BenefitConfiguration>> Select { get; set; }

            public List<(Expression<Func<BenefitConfiguration, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateBenefitConfigurationQuery(List<BenefitConfigurationDto> BenefitConfigurationToValidate) : IRequest<List<BenefitConfigurationDto>>
        {
            public List<BenefitConfigurationDto> BenefitConfigurationToValidate { get; } = BenefitConfigurationToValidate;
        }

        public class ValidateBenefitConfigurationQuery(Expression<Func<BenefitConfiguration, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<BenefitConfiguration, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET Education Program Detail

        #region CREATE Education Program

        public class CreateBenefitConfigurationRequest(BenefitConfigurationDto BenefitConfigurationDto) : IRequest<BenefitConfigurationDto>
        {
            public BenefitConfigurationDto BenefitConfigurationDto { get; set; } = BenefitConfigurationDto;
        }

        public class CreateListBenefitConfigurationRequest(List<BenefitConfigurationDto> BenefitConfigurationDtos) : IRequest<List<BenefitConfigurationDto>>
        {
            public List<BenefitConfigurationDto> BenefitConfigurationDtos { get; set; } = BenefitConfigurationDtos;
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public class UpdateBenefitConfigurationRequest(BenefitConfigurationDto BenefitConfigurationDto) : IRequest<BenefitConfigurationDto>
        {
            public BenefitConfigurationDto BenefitConfigurationDto { get; set; } = BenefitConfigurationDto;
        }

        public class UpdateListBenefitConfigurationRequest(List<BenefitConfigurationDto> BenefitConfigurationDtos) : IRequest<List<BenefitConfigurationDto>>
        {
            public List<BenefitConfigurationDto> BenefitConfigurationDtos { get; set; } = BenefitConfigurationDtos;
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public class DeleteBenefitConfigurationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Education Program
    }
}