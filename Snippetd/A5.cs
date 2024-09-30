var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDto.Adapt<CreateUpdatePatientFamilyRelationDto>().Adapt<PatientFamilyRelation>());

var result = await _unitOfWork.Repository<PatientFamilyRelation>().UpdateAsync(request.PatientFamilyRelationDtos.Adapt<List<PatientFamilyRelation>>());
