var result = await _unitOfWork.Repository<Family>().UpdateAsync(request.FamilyDto.Adapt<CreateUpdateFamilyDto>().Adapt<Family>());

var result = await _unitOfWork.Repository<Family>().UpdateAsync(request.FamilyDtos.Adapt<List<Family>>());
