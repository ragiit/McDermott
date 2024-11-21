namespace McDermott.Application.Features.Commands.Pharmacies
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

        public class DeleteReorderingRuleRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}