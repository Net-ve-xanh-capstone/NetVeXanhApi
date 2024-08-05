using Application.IService.IValidationService;
using Application.ViewModels.AccountViewModels;
using AutoMapper;

namespace Application.Services.ValidationService;

public class AccountValidationService : IAccountValidationService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AccountValidationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.AccountRepo.IsExistIdAsync(id);
    }

    public async Task<bool> IsExistedCompetitor(Guid id)
    {
        return await _unitOfWork.AccountRepo.IsExistCompetitor(id);
    }

    public async Task<bool> IsExistStaff(Guid id)
    {
        return await _unitOfWork.AccountRepo.IsExistStaff(id);
    }


    //Check Phone Exist
    public async Task<bool> IsExistPhone(string phone)
    {
        return await _unitOfWork.AccountRepo.IsExistPhone(phone);
    }

    //Check Email Exist
    public async Task<bool> IsExistEmail(string email)
    {
        return await _unitOfWork.AccountRepo.IsExistEmail(email);
    }

    //Check Username Exist
    public async Task<bool> IsExistUsername(string username)
    {
        return await _unitOfWork.AccountRepo.IsExistUsername(username);
    }

    public async Task<AccountValidationInfoViewModel> GetAccountByPaintingId(Guid paintingId)
    {
        var result = await _unitOfWork.PaintingRepo.GetAccountByPaintingIdAsync(paintingId);
        return _mapper.Map<AccountValidationInfoViewModel>(result);
    }
}