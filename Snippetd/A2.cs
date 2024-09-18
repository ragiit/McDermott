public async Task<List<DistrictDto>> Handle(BulkValidateDistrictQuery request, CancellationToken cancellationToken)
{
    var DistrictDtos = request.DistrictsToValidate;

    // Ekstrak semua kombinasi yang akan dicari di database
    var DistrictNames = DistrictDtos.Select(x => x.Name).Distinct().ToList();
    var postalCodes = DistrictDtos.Select(x => x.PostalCode).Distinct().ToList();
    var provinceIds = DistrictDtos.Select(x => x.ProvinceId).Distinct().ToList();
    var cityIds = DistrictDtos.Select(x => x.CityId).Distinct().ToList();
    var districtIds = DistrictDtos.Select(x => x.DistrictId).Distinct().ToList();

    var existingDistricts = await _unitOfWork.Repository<District>()
        .Entities
        .AsNoTracking()
        .Where(v => DistrictNames.Contains(v.Name)
                    && postalCodes.Contains(v.PostalCode)
                    && provinceIds.Contains(v.ProvinceId)
                    && cityIds.Contains(v.CityId)
                    && districtIds.Contains(v.DistrictId))
        .ToListAsync(cancellationToken);

    return existingDistricts.Adapt<List<DistrictDto>>();
}

public class BulkValidateDistrictQuery(List<DistrictDto> DistrictsToValidate) : IRequest<List<DistrictDto>>
{
    public List<DistrictDto> DistrictsToValidate { get; } = DistrictsToValidate;
}


IRequestHandler<BulkValidateDistrictQuery, List<DistrictDto>>,