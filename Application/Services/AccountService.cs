using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.AccountSendModels;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;

    // adding mapper in user service using DI
    private readonly IMapper _mapper;
    private readonly ISessionServices _sessionServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public AccountService(IUnitOfWork unitOfWork, ICurrentTime currentTime,
        IConfiguration configuration, ISessionServices sessionServices,
        IClaimsService claimsService, IMapper mapper, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _currentTime = currentTime;
        _configuration = configuration;
        _sessionServices = sessionServices;
        _claimsService = claimsService;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    public Task<bool?> CreateSubAccount(SubAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<(List<AccountViewModel>, int)> GetListExaminer(ListModels listModels)
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList
            .Where(x => x.Role == Role.Examiner.ToString()).ToList();
        if (accountList.Count == 0) throw new Exception("Không tìm thấy giám khảo nào.");
        var result = _mapper.Map<List<AccountViewModel>>(accountList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (result, totalPages);
    }

    public async Task<List<AccountViewModel>> GetAllExaminer()
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList
            .Where(x => x.Role == Role.Examiner.ToString()).ToList();
        if (accountList.Count == 0) throw new Exception("Không tìm thấy giám khảo nào.");
        var result = _mapper.Map<List<AccountViewModel>>(accountList);

        return result;
    }

    public async Task<(List<AccountViewModel>, int)> GetListCompetitor(ListModels listModels)
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList
            .Where(x => x.Role == Role.Competitor.ToString()).ToList();
        if (accountList.Count == 0) throw new Exception("Không tìm thấy thí sinh nào.");
        var result = _mapper.Map<List<AccountViewModel>>(accountList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (result, totalPages);
    }

    public async Task<List<AccountViewModel>> GetAllCompetitor()
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList
            .Where(x => x.Role == Role.Competitor.ToString()).ToList();
        if (accountList.Count == 0) throw new Exception("Không tìm thấy thí sinh nào.");
        var result = _mapper.Map<List<AccountViewModel>>(accountList);

        return result;
    }

    public async Task<(List<AccountViewModel>, int)> GetListStaff(ListModels listModels)
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList
            .Where(x => x.Role == Role.Staff.ToString()).ToList();
        if (accountList.Count == 0) throw new Exception("Không tìm thấy nhân viên nào.");
        var result = _mapper.Map<List<AccountViewModel>>(accountList);


        var totalPages = (int)Math.Ceiling((double)result.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (result, totalPages);
    }

    public async Task<List<AccountViewModel>> GetAllStaff()
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList
            .Where(x => x.Role == Role.Staff.ToString()).ToList();
        if (accountList.Count == 0) throw new Exception("Không tìm thấy nhân viên nào.");
        var result = _mapper.Map<List<AccountViewModel>>(accountList);


        return result;
    }

    public async Task<(List<AccountViewModel>, int)> GetListInactiveAccount(ListModels listModels)
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList.Where(x => x.Status == AccountStatus.Inactive.ToString()).ToList();
        var result = _mapper.Map<List<AccountViewModel>>(accountList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (result, totalPages);
    }

    public async Task<AccountViewModel?> GetAccountById(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdActiveAsync(id);
        if (account == null) throw new Exception("Không tìm thấy Account");
        return _mapper.Map<AccountViewModel>(account);
    }

    public async Task<AccountViewModel?> GetCompetitorById(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetCompetitorByIdAsync(id);
        if (account == null) throw new Exception("Không tìm thấy Account");
        return _mapper.Map<AccountViewModel>(account);
    }

    public async Task<AccountViewModel?> GetAccountByCode(string code)
    {
        var account = await _unitOfWork.AccountRepo.GetAccountByCodeAsync(code);
        if (account == null) throw new Exception("Không tìm thấy Account");
        return _mapper.Map<AccountViewModel>(account);
    }

    public async Task<bool?> UpdateAccount(AccountUpdateRequest updateAccount)
    {
        var validationResult = await ValidateAccountUpdateRequest(updateAccount);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);

        var account = await _unitOfWork.AccountRepo.GetByIdActiveAsync(updateAccount.Id);
        if (account == null) throw new Exception("Không tìm thấy tài khoản");
        _mapper.Map(updateAccount, account);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool?> InactiveAccount(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdActiveAsync(id);
        account.Status = AccountStatus.Inactive.ToString();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool?> ActiveAccount(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(id);
        account.Status = AccountStatus.Active.ToString();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<List<ContestRewardViewModel>> ListAccountHaveAwardIn3NearestContest()
    {
        var listContestId = await _unitOfWork.ContestRepo.Get3NearestContestId();
        if (listContestId.Count == 0) throw new Exception("Không tìm thấy cuộc thi");

        var listContestAward = await _unitOfWork.ContestRepo.GetContestRewardByListContestId(listContestId);
        if (listContestAward.Count == 0) throw new Exception("Không tìm thấy Account");

        return _mapper.Map<List<ContestRewardViewModel>>(listContestAward);
    }


    #region Validate

    public async Task<ValidationResult> ValidateAccountUpdateRequest(AccountUpdateRequest account)
    {
        return await _validatorFactory.AccountUpdateRequestValidator.ValidateAsync(account);
    }

    public async Task<ValidationResult> ValidateSubAccountRequest(SubAccountRequest accountUpdate)
    {
        return await _validatorFactory.SubAccountRequestValidator.ValidateAsync(accountUpdate);
    }

    #endregion
}