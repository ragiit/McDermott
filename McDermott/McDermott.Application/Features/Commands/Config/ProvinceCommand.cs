namespace McDermott.Application.Features.Commands.Config
{
    public class ProvinceCommand
    {
        public class GetProvinceQuery : IRequest<List<ProvinceDto>>;

        public class GetProvinceByIdQuery(int id) : IRequest<ProvinceDto>
        {
            public int Id { get; set; } = id;
        }

        public class GetProvinceByCountry(int? countryId) : IRequest<List<ProvinceDto>>
        {
            public int? CountryId { get; set; } = countryId;
        }

        public class CreateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class UpdateProvinceRequest(ProvinceDto ProvinceDto) : IRequest<bool>
        {
            public ProvinceDto ProvinceDto { get; set; } = ProvinceDto;
        }

        public class DeleteProvinceRequest(int id) : IRequest<bool>
        {
            public int Id { get; set; } = id;
        }

        public class DeleteListProvinceRequest(List<int> id) : IRequest<bool>
        {
            public List<int> Id { get; set; } = id;
        }
    }
}