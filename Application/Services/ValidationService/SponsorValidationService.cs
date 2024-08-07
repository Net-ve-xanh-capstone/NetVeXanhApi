﻿using Application.IService.IValidationService;

namespace Application.Services.ValidationService;

public class SponsorValidationService : ISponsorValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public SponsorValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.SponsorRepo.IsExistIdAsync(id);
    }
}