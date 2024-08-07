﻿using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class ContestValidationService : IContestValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ContestValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ContestRepo.IsExistIdAsync(id);
    }
}