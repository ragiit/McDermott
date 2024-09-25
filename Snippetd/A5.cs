var result = await _unitOfWork.Repository<JobPosition>().UpdateAsync(request.JobPositionDto.Adapt<CreateUpdateJobPositionDto>().Adapt<JobPosition>());

var result = await _unitOfWork.Repository<JobPosition>().UpdateAsync(request.JobPositionDtos.Adapt<List<JobPosition>>());
