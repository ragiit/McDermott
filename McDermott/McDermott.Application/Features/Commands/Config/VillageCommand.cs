namespace McDermott.Application.Features.Commands.Config
{
    public class VillageCommand
    {
        public class GetVillageQuery : IRequest<List<VillageDto>>;

        public class GetVillageByIdQuery : IRequest<VillageDto>
        {
            public long Id { get; set; }

            public GetVillageByIdQuery(long id)
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
            public long Id { get; set; }

            public DeleteVillageRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListVillageRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListVillageRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}