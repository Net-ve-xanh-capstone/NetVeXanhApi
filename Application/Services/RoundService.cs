using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Round;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.RoundViewModels;
using Application.ViewModels.TopicViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class RoundService : IRoundService
{
    private readonly IExcelService _excelService;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public RoundService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IExcelService excelService ,IValidatorFactory validatorFactory)
    {
        _excelService = excelService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _validatorFactory = validatorFactory;
    }

    #region Create

    public async Task<bool> CreateRound(CreateRoundRequest model)
    {
        foreach (var level in model.LevelList)
        {
            var educationalLevel = await _unitOfWork.EducationalLevelRepo.GetByIdAsync(level);
            var newRound = _mapper.Map<Round>(model);

            // Kiểm tra trùng lặp thời gian với các vòng thi hiện có
            if (educationalLevel!.Round.Any(r =>
                    (model.StartTime >= r.StartTime && model.StartTime <= r.EndTime) ||
                    (model.EndTime >= r.StartTime && model.EndTime <= r.EndTime) ||
                    (model.StartTime <= r.StartTime && model.EndTime >= r.EndTime)))
            {
                throw new Exception("Thời gian bắt đầu và kết thúc bị trùng với vòng thi khác.");
            }

            // Kiểm tra thời gian bắt đầu và kết thúc của vòng thi mới có nằm trong khoảng thời gian của cuộc thi không
            if (model.StartTime < educationalLevel!.Contest.StartTime ||
                model.EndTime > educationalLevel.Contest.EndTime)
            {
                throw new Exception(
                    "Thời gian bắt đầu và kết thúc của vòng thi không nằm trong khoảng thời gian của cuộc thi.");
            }

            foreach (var award in newRound.Award)
            {
                award.CreatedBy = newRound.CreatedBy;
            }

            var listOldRound = educationalLevel!.Round.Where(src => src.RoundNumber >= model.RoundNumber).ToList();


            foreach (var round in listOldRound)
            {
                round.RoundNumber += 1;
            }

            educationalLevel.Round.Add(newRound);
            _unitOfWork.EducationalLevelRepo.Update(educationalLevel);
        }
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get All Round

    public async Task<List<RoundResponse>> GetListRound(ListModels listModels)
    {
        var list = await _unitOfWork.RoundRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Round nao");

        return _mapper.Map<List<RoundResponse>>(list);
    }

    #endregion

    #region Get By Id

    public async Task<RoundResponse?> GetRoundById(Guid id)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(id);
        if (round == null) throw new Exception("Khong tim thay Round");

        return _mapper.Map<RoundResponse>(round);
    }

    #endregion

    #region Update

    public async Task<bool> UpdateRound(RoundUpdateRequest updateRound)
    {
        var validationResult = await ValidateRoundUpdateRequest(updateRound);
        if (!validationResult.IsValid)
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(updateRound.Id);
        if (round == null) throw new Exception("Khong tim thay Round");
        _mapper.Map(updateRound, round);
        round.UpdatedBy = updateRound.CurrentUserId;
        round.UpdatedTime = DateTime.Now;

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeleteRound(Guid id)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(id);
        if (round == null) throw new Exception("Khong tim thay Round");
        round.Status = RoundStatus.Delete.ToString();
        foreach (var schedule in round.Schedule) schedule.Status = ScheduleStatus.Delete.ToString();
        foreach (var award in round.Award) award.Status = AwardStatus.Inactive.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get Topic

    public async Task<(List<TopicResponse>, int)> GetTopicInRound(Guid id, ListModels listModels)
    {
        var list = await _unitOfWork.RoundRepo.GetTopic(id);
        if (list.Count == 0) throw new Exception("Khong tim thay Topic nao trong Round");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<TopicResponse>>(result), totalPages);
    }

    #endregion

    #region Get Round By Educational LevelId

    public async Task<(List<RoundResponse>, int)> GetRoundByEducationalLevelId(ListModels listRoundModel, Guid levelId)
    {
        var list = await _unitOfWork.RoundRepo.GetRoundByLevelId(levelId);
        if (list.Count == 0) throw new Exception("Khong tim thay Round nao");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listRoundModel.PageSize);
        int? itemsToSkip = (listRoundModel.PageNumber - 1) * listRoundModel.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listRoundModel.PageSize)
            .ToList();
        return (_mapper.Map<List<RoundResponse>>(result), totalPages);
    }

    #endregion

    #region Get

    public async Task<List<RoundResponse>> GetListRoundForCompetitor()
    {
        var today = _currentTime.GetCurrentTime();
        var result = await _unitOfWork.RoundRepo.GetRoundsOfThisYear();
        return _mapper.Map<List<RoundResponse>>(result);
    }

    #endregion

    #region Export

    public async Task<(byte[], string)> GetListCompetitorOfRound(Guid roundId)
    {
        var round = await _unitOfWork.RoundRepo.GetRoundDetail(roundId);
        var competitors = _mapper.Map<List<CompetitorResponse>>(round); 
        var result = await _excelService.GenerateExcel(competitors, round.Name);
        return (result,  round.Name);
    }

    #endregion 


    #region Validate
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.RoundRepo.IsExistIdAsync(id);
    }
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