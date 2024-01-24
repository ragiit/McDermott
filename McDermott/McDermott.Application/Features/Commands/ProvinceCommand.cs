namespace McDermott.Application.Features.Commands
{
    public class ProvinceCommand
    {
        public class GetProvinceQuery : IRequest<List<ProvinceDto>>;
       

        public class GetProvinceByIdQuery : IRequest<ProvinceDto>
        {
            public int Id { get; set; }

            public GetProvinceByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class GetProvinceByCountry : IRequest<List<ProvinceDto>>
        {
            public int? CountryId { get; set; }

            public GetProvinceByCountry(int? countryId)
            {
                CountryId = countryId;
            }
        }

        public class CreateProvinceRequest : IRequest<ProvinceDto>
        {
            public ProvinceDto ProvinceDto { get; set; }

            public CreateProvinceRequest(ProvinceDto ProvinceDto)
            {
                this.ProvinceDto = ProvinceDto;
            }
        }

        public class UpdateProvinceRequest : IRequest<bool>
        {
            public ProvinceDto ProvinceDto { get; set; }

            public UpdateProvinceRequest(ProvinceDto ProvinceDto)
            {
                this.ProvinceDto = ProvinceDto;
            }
        }

        public class DeleteProvinceRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteProvinceRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListProvinceRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListProvinceRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}