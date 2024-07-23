using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Round;
using Application.SendModels.Topic;
using Application.ViewModels.RoundViewModels;
using Application.ViewModels.TopicViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation.Results;
using Infracstructures;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class RoundService : IRoundService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public RoundService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration,
        IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Create

    public async Task<bool> CreateRound(RoundRequest round)
    {
        var listNewRound = new List<Round>();
        foreach (var id in round.listLevel)
        {
            var newRound = _mapper.Map<Round>(round);
            newRound.Status = RoundStatus.Active.ToString();
            newRound.EducationalLevelId = id;
            newRound.CreatedTime = _currentTime.GetCurrentTime();
            newRound.UpdatedTime = _currentTime.GetCurrentTime();
            listNewRound.Add(newRound);
        }
        await _unitOfWork.RoundRepo.AddRangeAsync(listNewRound);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get All Round

    public async Task<List<RoundViewModel>> GetListRound(ListModels listModels)
    {
        var list = await _unitOfWork.RoundRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Round nao");
        
        return _mapper.Map<List<RoundViewModel>>(list);
    }
    
    #endregion

    #region Get By Id

    public async Task<RoundViewModel?> GetRoundById(Guid id)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(id);
        if (round == null) throw new Exception("Khong tim thay Round");

        return _mapper.Map<RoundViewModel>(round);
    }

    #endregion

    #region Update

    public async Task<bool> UpdateRound(RoundUpdateRequest updateRound)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(updateRound.Id);
        if (round == null) throw new Exception("Khong tim thay Round");
        _mapper.Map(updateRound, round);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteRound(Guid id)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(id);
        if (round == null) throw new Exception("Khong tim thay Round");
        round.Status = RoundStatus.Inactive.ToString();
        foreach (var schedule in round.Schedule) schedule.Status = ScheduleStatus.Delete.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get Topic

    public async Task<(List<TopicViewModel>, int)> GetTopicInRound(Guid id, ListModels listModels)
    {
        var list = await _unitOfWork.RoundRepo.GetTopic(id);
        if (list.Count == 0) throw new Exception("Khong tim thay Topic nao trong Round");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<TopicViewModel>>(result), totalPages);
    }

    #endregion

    #region Get Round By Educational LevelId

    public async Task<(List<RoundViewModel>, int)> GetRoundByEducationalLevelId(ListModels listRoundModel, Guid levelId)
    {
        var list = await _unitOfWork.RoundRepo.GetRoundByLevelId(levelId);
        if (list.Count == 0) throw new Exception("Khong tim thay Round nao");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listRoundModel.PageSize);
        int? itemsToSkip = (listRoundModel.PageNumber - 1) * listRoundModel.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listRoundModel.PageSize)
            .ToList();
        return (_mapper.Map<List<RoundViewModel>>(result), totalPages);
    }

    #endregion

    #region Get 
    public async Task<List<RoundViewModel>> GetListRoundForCompetitor()
    {
        var today = _currentTime.GetCurrentTime();
        var result = await _unitOfWork.RoundRepo.GetRoundsOfThisYear();
        if (result[1].EducationalLevel.Contest.StartTime >= today || today >= result[1].EducationalLevel.Contest.EndTime)
        {
            return null;
        }
        return _mapper.Map<List<RoundViewModel>>(result);
    }
    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.RoundRepo.IsExistIdAsync(id);
    }


    #region Validate
    public async Task<ValidationResult> ValidateRoundRequest(RoundRequest round)
    {
        return await _validatorFactory.RoundRequestValidator.ValidateAsync(round);
    }

    public async Task<ValidationResult> ValidateRoundUpdateRequest(RoundUpdateRequest roundUpdate)
    {
        return await _validatorFactory.RoundUpdateRequestValidator.ValidateAsync(roundUpdate);
    }
    #endregion
}