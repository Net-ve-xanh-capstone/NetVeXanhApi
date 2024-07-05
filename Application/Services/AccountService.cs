﻿using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.AccountSendModels;
using Application.ViewModels.AccountViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;
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

    public AccountService(IUnitOfWork unitOfWork, ICurrentTime currentTime,
        IConfiguration configuration, ISessionServices sessionServices,
        IClaimsService claimsService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentTime = currentTime;
        _configuration = configuration;
        _sessionServices = sessionServices;
        _claimsService = claimsService;
        _mapper = mapper;
    }

    public Task<bool?> CreateSubAccount(SubAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<(List<AccountViewModel>, int)> GetListExaminer(ListModels listModels)
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList.Where(x => x.Role == Role.Examiner.ToString()).ToList();
        var result = _mapper.Map<List<AccountViewModel>>(accountList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (result, totalPages);
    }
    
    public async Task<(List<AccountViewModel>, int)> GetListCompetitor(ListModels listModels)
    {
        var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
        accountList = accountList.Where(x => x.Role == Role.Competitor.ToString()).ToList();
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
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(id);
        if (account == null || account.Status == AccountStatus.Inactive.ToString())
        {
            return null;
        }
        return _mapper.Map<AccountViewModel>(account);
    }

    public async Task<bool?> UpdateAccount(AccountUpdateRequest updateAccount)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(updateAccount.Id);
        if (account == null) throw new Exception("Khong tim thay account");
        _mapper.Map(updateAccount, account);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteAccount(Guid id)
    {
        var account = await _unitOfWork.AccountRepo.GetByIdAsync(id);
        account.Status = AccountStatus.Inactive.ToString();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}