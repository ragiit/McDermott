namespace McDermott.Application.Features.Commands.Config
{
    public class ProvinceCommand
    {
        public class GetProvinceQuery : IRequest<List<ProvinceDto>>;

        public class GetProvinceByIdQuery(long id) : IRequest<ProvinceDto>
        {
             public long Id { get; set; } = id;
        }

        public class GetProvinceByCountry(long? countryId) : IRequest<List<ProvinceDto>>
        {
            public long? CountryId { get; set; } = countryId;
        }

        public class CreateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class UpdateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<bool>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class DeleteProvinceRequest(long id) : IRequest<bool>
        {
             public long Id { get; set; } = id;
        }

        public class DeleteListProvinceRequest(List<long> id) : IRequest<bool>
        {
            public List<long> Id { get; set; } = id;
        }
    }
}