namespace McDermott.Application.Features.Commands.Medical
{
    public class DoctorScheduleCommand
    {
        #region Get

        //public class GetDoctorScheduleQuery : IRequest<List<DoctorScheduleDto>>;
        public class GetDoctorScheduleQuery(Expression<Func<DoctorSchedule, bool>>? predicate = null) : IRequest<List<DoctorScheduleDto>>
        {
            public Expression<Func<DoctorSchedule, bool>> Predicate { get; } = predicate;
        }

        //public class GetDoctorScheduleSlotQuery : IRequest<List<DoctorScheduleSlotDto>>;

        public class GetDoctorScheduleByIdQuery : IRequest<DoctorScheduleDto>
        {
            public long Id { get; set; }

            public GetDoctorScheduleByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class GetDoctorScheduleSlotByDoctorScheduleIdRequest : IRequest<List<DoctorScheduleSlotDto>>
        {
            public long DoctorScheduleId { get; set; }
            public long PhysicianId { get; set; }

            public GetDoctorScheduleSlotByDoctorScheduleIdRequest(long DoctorScheduleId, long PhysicianId)
            {
                this.DoctorScheduleId = DoctorScheduleId;
                this.PhysicianId = PhysicianId;
            }

            public GetDoctorScheduleSlotByDoctorScheduleIdRequest(long DoctorScheduleId)
            {
                this.DoctorScheduleId = DoctorScheduleId;
            }
        }

        public class GetGetScheduleDetailQuery(Expression<Func<DoctorScheduleDetail, bool>>? predicate = null) : IRequest<List<DoctorScheduleDetailDto>>
        {
            public Expression<Func<DoctorScheduleDetail, bool>> Predicate { get; } = predicate;
        }

        public class GetDoctorScheduleDetailByScheduleIdQuery : IRequest<List<DoctorScheduleDetailDto>>
        {
            public long DoctorScheduleId { get; set; }

            public GetDoctorScheduleDetailByScheduleIdQuery(long DoctorScheduleId)
            {
                this.DoctorScheduleId = DoctorScheduleId;
            }
        }

        #endregion Get

        #region Create

        //public class CreateDoctorScheduleDetailRequest : IRequest<bool>
        //{
        //    public List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos { get; set; }

        //    public CreateDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos)
        //    {
        //        this.DoctorScheduleDetailDtos = DoctorScheduleDetailDtos;
        //    }
        //}

        public class CreateDoctorScheduleRequest : IRequest<DoctorScheduleDto>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; }

            public CreateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto)
            {
                this.DoctorScheduleDto = DoctorScheduleDto;
            }
        }

        #endregion Create

        #region Update

        public class UpdateDoctorScheduleRequest : IRequest<bool>
        {
            public DoctorScheduleDto DoctorScheduleDto { get; set; }

            public UpdateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto)
            {
                this.DoctorScheduleDto = DoctorScheduleDto;
            }
        }

        #endregion Update

        #region Delete

        public class DeleteDoctorScheduleDetailByScheduleIdRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteDoctorScheduleDetailByScheduleIdRequest(List<long> Id)
            {
                this.Id = Id;
            }
        }

        public class DeleteDoctorScheduleRequest : IRequest<bool>
        {
            public long Id { get; set; }

            public DeleteDoctorScheduleRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteDoctorScheduleLocationByIdRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteDoctorScheduleLocationByIdRequest(List<long> Id)
            {
                this.Id = Id;
            }
        }

        public class DeleteListDoctorScheduleRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListDoctorScheduleRequest(List<long> id)
            {
                this.Id = id;
            }
        }

        public class DeleteListDoctorScheduleSlotRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListDoctorScheduleSlotRequest(List<long> id)
            {
                this.Id = id;
            }
        }

        public class DeleteDoctorScheduleSlotByPhysicionIdRequest : IRequest<bool>
        {
            public List<long> PhysicianIds { get; set; }
            public long DoctorScheduleId { get; set; }

            public DeleteDoctorScheduleSlotByPhysicionIdRequest(List<long> PhysicianIds, long DoctorScheduleId)
            {
                this.PhysicianIds = PhysicianIds;
                this.DoctorScheduleId = DoctorScheduleId;
            }
        }

        public class DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest : IRequest<bool>
        {
            public List<long> ScheduleId { get; set; }
            public long PhysicionId { get; set; }

            public DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest(List<long> ScheduleId, long physicionId)
            {
                this.ScheduleId = ScheduleId;
                this.PhysicionId = physicionId;
            }
        }

        public class DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest : IRequest<bool>
        {
            public List<long> ScheduleId { get; set; }
            public List<long> PhysicionId { get; set; }

            public DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest(List<long> scheduleId, List<long> physicionId)
            {
                ScheduleId = scheduleId;
                PhysicionId = physicionId;
            }
        }

        #endregion Delete
    }
}