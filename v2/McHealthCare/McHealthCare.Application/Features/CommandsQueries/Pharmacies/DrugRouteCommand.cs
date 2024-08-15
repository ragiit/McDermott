namespace McHealthCare.Application.Features.Commands.Pharmacies
{
    public class DrugRouteCommand
    {
        #region GET

        public class GetDrugRouteQuery(Expression<Func<DrugRoute, bool>>? predicate = null, bool removeCache = false) : IRequest<List<DrugRouteDto>>
        {
            public Expression<Func<DrugRoute, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateDrugRouteRequest(DrugRouteDto DrugRouteDto) : IRequest<DrugRouteDto>
        {
            public DrugRouteDto DrugRouteDto { get; set; } = DrugRouteDto;
        }

        public class CreateListDrugRouteRequest(List<DrugRouteDto> DrugRoutetos) : IRequest<List<DrugRouteDto>>
        {
            public List<DrugRouteDto> DrugRouteDtos { get; set; } = DrugRoutetos;
        }

        #endregion CREATE

        #region Update

        public class UpdateDrugRouteRequest(DrugRouteDto DrugRouteDto) : IRequest<DrugRouteDto>
        {
            public DrugRouteDto DrugRouteDto { get; set; } = DrugRouteDto;
        }

        public class UpdateListDrugRouteRequest(List<DrugRouteDto> DrugRouteDtos) : IRequest<List<DrugRouteDto>>
        {
            public List<DrugRouteDto> DrugRouteDtos { get; set; } = DrugRouteDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteDrugRouteRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}