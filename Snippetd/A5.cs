var result = await _unitOfWork.Repository<User>().UpdateAsync(request.UserDto.Adapt<CreateUpdateUserDto>().Adapt<User>());

var result = await _unitOfWork.Repository<User>().UpdateAsync(request.UserDtos.Adapt<List<User>>());


 var createUpdateDtos = request.PatientFamilyRelationDto.Adapt<List<CreateUpdatePatientFamilyRelationDto>>();
 var patientFamilyRelations = createUpdateDtos.Adapt<List<PatientFamilyRelation>>();
 var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(patientFamilyRelations);


  public async Task<List<PatientFamilyRelationDto>> Handle(CreateListPatientFamilyRelationRequest request, CancellationToken cancellationToken)
 {
     try
     {
         var createUpdateDtos = request.PatientFamilyRelationDto.Adapt<List<CreateUpdatePatientFamilyRelationDto>>();
         var patientFamilyRelations = createUpdateDtos.Adapt<List<PatientFamilyRelation>>();
         var result = await _unitOfWork.Repository<PatientFamilyRelation>().AddAsync(patientFamilyRelations);

         await _unitOfWork.SaveChangesAsync(cancellationToken);

         _cache.Remove("GetPatientFamilyRelationQuery_"); // Ganti dengan key yang sesuai

         return result.Adapt<List<PatientFamilyRelationDto>>();
     }
     catch (Exception)
     {
         throw;
     }
 }