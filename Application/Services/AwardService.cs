using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Award;
using Application.ViewModels.AwardViewModels;
using AutoMapper;
using Domain;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class AwardService : IAwardService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public AwardService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IConfiguration configuration,
        IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
        

    }

    #region Add Award

    public async Task<bool> AddAward(CreateAwardRequest model)
    {
        var round = await _unitOfWork.RoundRepo.GetByIdAsync(model.RoundId);
        if (round!.Name.ToLower() != "Vòng Chung Kết".ToLower())
        {
            if (round.Award.Where(x=>x.Status != AwardStatus.Inactive.ToString()).Count() > 0) throw new Exception("Các vòng khác vòng chung kết chỉ được có 1 giải"); 
        }
        if (round.Award.Any(src => src.Rank == model.Rank && src.Status == "Active"))
            throw new Exception(" Bạn không thể thêm được các giải đã có sẵn");
        var validationResult = await ValidateAwardRequest(model);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        var award = _mapper.Map<Award>(model);
        /*        award.Rank = RankAward.OtherAward.ToString();
                award.Description = model.Rank;*/
        award.Status = AwardStatus.Active.ToString();
        await _unitOfWork.AwardRepo.AddAsync(award);
        award.CreatedTime = _currentTime.GetCurrentTime();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get List Award

    public async Task<(List<AwardViewResponse>, int)> GetListAward(ListModels listAwardModel)
    {
        var awardList = await _unitOfWork.AwardRepo.GetAllAsync();
        if (awardList.Count == 0) throw new Exception("Không có giải thưởng nào");
        var result = _mapper.Map<List<AwardViewResponse>>(awardList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listAwardModel.PageSize);
        int? itemsToSkip = (listAwardModel.PageNumber - 1) * listAwardModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listAwardModel.PageSize)
            .ToList();
        return (result, totalPages);
    }

    #endregion

    #region Get List Award By ContestId

    public async Task<ListAwardForCreateSchedule?> GetListAwardsByRoundIdForSchedule(Guid roundId)
    {
        var result = new ListAwardForCreateSchedule();
        result.paintingForSchedule = await _unitOfWork.PaintingRepo.GetNumPaintingInRoundIsNotHaveSchedule(roundId);

                var list = await _unitOfWork.AwardRepo.GetAwardsByRoundId(roundId);
        foreach (var a in list)
        {
            var count = 0;
            foreach (var awardSchedule in a.AwardSchedule)
            {
                count = count + awardSchedule.Quantity;
            }
            a.Quantity = a.Quantity - count;

        }
        result.listAward = _mapper.Map<List<AwardViewResponse>>(list);

        return result;
    }

    #endregion

    #region Get List Award By ContestId

    public async Task<List<AwardViewResponse>?> GetAwardsByRoundId(Guid roundId)
    {
        var list = await _unitOfWork.AwardRepo.GetAwardsByRoundId(roundId);

        return _mapper.Map<List<AwardViewResponse>>(list);
    }

    #endregion

    #region Delete Award

    public async Task<bool> DeleteAward(Guid awardId)
    {
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(awardId);
        if (award == null) throw new Exception("Không tìm thấy giải thưởng");
        award.Status = AwardStatus.Inactive.ToString();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Award

    public async Task<bool> UpdateAward(UpdateAwardRequest updateAward)
    {
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(updateAward.Id);

        if (award == null) throw new Exception("Không tìm thấy giải thưởng");
        if (award.Rank == RankAward.OtherAward.ToString())
        {
            _mapper.Map(updateAward, award);
            award.Rank = RankAward.OtherAward.ToString();
            award.Description = updateAward.Rank;
        }
        else
        {
            _mapper.Map(updateAward, award);
        }

        award.UpdatedTime = _currentTime.GetCurrentTime();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get Award By Id

    public async Task<AwardViewResponse> GetAwardById(Guid awardId)
    {
        var award = await _unitOfWork.AwardRepo.GetByIdAsync(awardId);
        if (award == null) throw new Exception("Không tìm thấy giải thưởng");
        return _mapper.Map<AwardViewResponse>(award);
    }

    #endregion

    #region IsExisted

    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.AwardRepo.IsExistIdAsync(id);
    }

    #endregion

    #region Validate

    public async Task<ValidationResult> ValidateAwardRequest(CreateAwardRequest createAward)
    {
        return await _validatorFactory.AwardRequestValidator.ValidateAsync(createAward);
    }

    public async Task<ValidationResult> ValidateTopicUpdateRequest(UpdateAwardRequest awardUpdate)
    {
        return await _validatorFactory.UpdateAwardRequestValidator.ValidateAsync(awardUpdate);
    }

    #endregion

}