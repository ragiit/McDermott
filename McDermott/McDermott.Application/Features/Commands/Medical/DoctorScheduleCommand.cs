using McDermott.Application.Dtos.Medical;

namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduleCommand
    { 
        #region Get
        public class GetDoctorScheduleQuery : IRequest<List<DoctorScheduleDto>>;
        public class GetDoctorScheduleSlotQuery : IRequest<List<DoctorScheduleSlotDto>>;

        public class GetDoctorScheduleByIdQuery : IRequest<DoctorScheduleDto>
        {
            public int Id { get; set; }

            public GetDoctorScheduleByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class GetDoctorScheduleSlotByDoctorScheduleIdRequest : IRequest<List<DoctorScheduleSlotDto>>
        {
            public int DoctorScheduleId { get; set; }
            public int PhysicianId { get; set; }

            public GetDoctorScheduleSlotByDoctorScheduleIdRequest(int DoctorScheduleId, int PhysicianId)
            {
                this.DoctorScheduleId = DoctorScheduleId;
                this.PhysicianId = PhysicianId;
            }
            public GetDoctorScheduleSlotByDoctorScheduleIdRequest(int DoctorScheduleId)
            {
                this.DoctorScheduleId = DoctorScheduleId;
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
        #endregion

        #region Create
        public class CreateDoctorScheduleDetailRequest : IRequest<bool>
        {
            public List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos { get; set; }

            public CreateDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos)
            {
                this.DoctorScheduleDetailDtos = DoctorScheduleDetailDtos;
            }
        }

        public class CreateDoctorScheduleSlotRequest : IRequest<bool>
        {
            public List<DoctorScheduleSlotDto> DoctorScheduleSlotDto { get; set; }

            public CreateDoctorScheduleSlotRequest(List<DoctorScheduleSlotDto> DoctorScheduleSlotDto)
            {
                this.DoctorScheduleSlotDto = DoctorScheduleSlotDto;
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
        #endregion

        #region Update
        public class UpdateDoctorScheduleRequest : IRequest<bool>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; }

            public UpdateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto)
            {
                this.DoctorScheduleDto = DoctorScheduleDto;
            }
        }
        #endregion

        #region Delete
        public class DeleteDoctorScheduleDetailByScheduleIdRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteDoctorScheduleDetailByScheduleIdRequest(List<int> Id)
            {
                this.Id = Id;
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

        public class DeleteDoctorScheduleLocationByIdRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteDoctorScheduleLocationByIdRequest(List<int> Id)
            {
                this.Id = Id;
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

        public class DeleteDoctorScheduleSlotRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteDoctorScheduleSlotRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListDoctorScheduleSlotRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDoctorScheduleSlotRequest(List<int> id)
            {
                this.Id = id;
            }
        }

        public class DeleteDoctorScheduleSlotByPhysicionIdRequest : IRequest<bool>
        {
            public List<int> PhysicianIds { get; set; }
            public int DoctorScheduleId { get; set; }   

            public DeleteDoctorScheduleSlotByPhysicionIdRequest(List<int> PhysicianIds, int DoctorScheduleId)
            {
                this.PhysicianIds = PhysicianIds;
                this.DoctorScheduleId = DoctorScheduleId;
            }
        }

        public class DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest : IRequest<bool>
        {
            public List<int> ScheduleId { get; set; }
            public int PhysicionId { get; set; }

            public DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest(List<int> ScheduleId, int physicionId)
            {
                this.ScheduleId = ScheduleId;
                this.PhysicionId = physicionId;
            }
        }
        public class DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest : IRequest<bool>
        {
            public List<int> ScheduleId { get; set; }
            public List<int> PhysicionId { get; set; }

            public DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest(List<int> scheduleId, List<int> physicionId)
            {
                ScheduleId = scheduleId;
                PhysicionId = physicionId;
            }
        }
        #endregion
    }
}