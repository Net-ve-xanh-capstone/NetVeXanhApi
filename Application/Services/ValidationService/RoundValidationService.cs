using Application.IService.IValidationService;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.ValidationService;

public class RoundValidationService : IRoundValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoundValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.RoundRepo.IsExistIdAsync(id);
    }

    public async Task<bool> BeInCompleteStatus(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(roundId);

        if (round == null)
        {
            throw new Exception("Round không tồn tại.");
        }

        return round.Status == RoundStatus.Complete.ToString();
    }
}