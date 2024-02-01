namespace McDermott.Application.Features.Commands
{
    public class DoctorScheduleCommand
    {
        public class GetDoctorScheduleQuery : IRequest<List<DoctorScheduleDto>>;

        public class GetDoctorScheduleByIdQuery : IRequest<DoctorScheduleDto>
        {
            public int Id { get; set; }

            public GetDoctorScheduleByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class GetDoctorScheduleDetailByScheduleIdQuery : IRequest<List<DoctorScheduleDetailDto>>
        {
            public int DoctorScheduleId { get; set; }

            public GetDoctorScheduleDetailByScheduleIdQuery(int DoctorScheduleId)
            {
                this.DoctorScheduleId = DoctorScheduleId;
            }
        }

        public class CreateDoctorScheduleDetailRequest : IRequest<bool>
        {
            public List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos { get; set; }

            public CreateDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos)
            {
                this.DoctorScheduleDetailDtos = DoctorScheduleDetailDtos;
            }
        }

        public class DeleteDoctorScheduleDetailByScheduleIdRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteDoctorScheduleDetailByScheduleIdRequest(List<int> Id)
            {
                this.Id = Id;
            }
        }

        public class CreateDoctorScheduleRequest : IRequest<DoctorScheduleDto>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; }

            public CreateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto)
            {
                this.DoctorScheduleDto = DoctorScheduleDto;
            }
        }

        public class DeleteDoctorScheduleLocationByIdRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteDoctorScheduleLocationByIdRequest(List<int> Id)
            {
                this.Id = Id;
            }
        }

        public class UpdateDoctorScheduleRequest : IRequest<bool>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; }

            public UpdateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto)
            {
                this.DoctorScheduleDto = DoctorScheduleDto;
            }
        }

        public class DeleteDoctorScheduleRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteDoctorScheduleRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListDoctorScheduleRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDoctorScheduleRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}