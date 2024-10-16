public async Task<List<DoctorScheduleSlotDto>> Handle(BulkValidateDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
{
    var DoctorScheduleSlotDtos = request.DoctorScheduleSlotsToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var DoctorScheduleSlotNames = DoctorScheduleSlotDtos.Select(x => x.Name).Distinct().ToList();
    var postalCodes = DoctorScheduleSlotDtos.Select(x => x.PostalCode).Distinct().ToList();
    var provinceIds = DoctorScheduleSlotDtos.Select(x => x.ProvinceId).Distinct().ToList();
    var cityIds = DoctorScheduleSlotDtos.Select(x => x.CityId).Distinct().ToList();
    var DoctorScheduleSlotIds = DoctorScheduleSlotDtos.Select(x => x.DoctorScheduleSlotId).Distinct().ToList();

    var existingDoctorScheduleSlots = await _unitOfWork.Repository<DoctorScheduleSlot>()
        .Entities
        .AsNoTracking()
        .Where(v => DoctorScheduleSlotNames.Contains(v.Name)
                    && postalCodes.Contains(v.PostalCode)
                    && provinceIds.Contains(v.ProvinceId)
                    && cityIds.Contains(v.CityId)
                    && DoctorScheduleSlotIds.Contains(v.DoctorScheduleSlotId))
        .ToListAsync(cancellationToken);

    return existingDoctorScheduleSlots.Adapt<List<DoctorScheduleSlotDto>>();
}

public class BulkValidateDoctorScheduleSlotQuery(List<DoctorScheduleSlotDto> DoctorScheduleSlotsToValidate) : IRequest<List<DoctorScheduleSlotDto>>
{
    public List<DoctorScheduleSlotDto> DoctorScheduleSlotsToValidate { get; } = DoctorScheduleSlotsToValidate;
}


IRequestHandler<BulkValidateDoctorScheduleSlotQuery, List<DoctorScheduleSlotDto>>,