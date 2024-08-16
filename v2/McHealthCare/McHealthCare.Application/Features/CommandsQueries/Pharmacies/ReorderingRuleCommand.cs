namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class ReorderingRuleCommand
    {
        #region GET

        public class GetReorderingRuleQuery(Expression<Func<ReorderingRule, bool>>? predicate = null, bool removeCache = false) : IRequest<List<ReorderingRuleDto>>
        {
            public Expression<Func<ReorderingRule, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET



        #region CREATE

        public class CreateReorderingRuleRequest(ReorderingRuleDto ReorderingRuleDto) : IRequest<ReorderingRuleDto>
        {
            public ReorderingRuleDto ReorderingRuleDto { get; set; } = ReorderingRuleDto;
        }

        public class CreateListReorderingRuleRequest(List<ReorderingRuleDto> ReorderingRuleDtos) : IRequest<List<ReorderingRuleDto>>
        {
            public List<ReorderingRuleDto> ReorderingRuleDtos { get; set; } = ReorderingRuleDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateReorderingRuleRequest(ReorderingRuleDto ReorderingRuleDto) : IRequest<ReorderingRuleDto>
        {
            public ReorderingRuleDto ReorderingRuleDto { get; set; } = ReorderingRuleDto;
        }

        public class UpdateListReorderingRuleRequest(List<ReorderingRuleDto> ReorderingRuleDtos) : IRequest<List<ReorderingRuleDto>>
        {
            public List<ReorderingRuleDto> ReorderingRuleDtos { get; set; } = ReorderingRuleDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteReorderingRuleRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}