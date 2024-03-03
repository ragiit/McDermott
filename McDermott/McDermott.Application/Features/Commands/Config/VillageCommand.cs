namespace McDermott.Application.Features.Commands.Config
{
    public class VillageCommand
    {
        public class GetVillageQuery : IRequest<List<VillageDto>>;

        public class GetVillageByIdQuery : IRequest<VillageDto>
        {
            public int Id { get; set; }

            public GetVillageByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateVillageRequest : IRequest<VillageDto>
        {
            public VillageDto VillageDto { get; set; }

            public CreateVillageRequest(VillageDto VillageDto)
            {
                this.VillageDto = VillageDto;
            }
        }

        public class UpdateVillageRequest : IRequest<bool>
        {
            public VillageDto VillageDto { get; set; }

            public UpdateVillageRequest(VillageDto VillageDto)
            {
                this.VillageDto = VillageDto;
            }
        }

        public class DeleteVillageRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteVillageRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListVillageRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListVillageRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}