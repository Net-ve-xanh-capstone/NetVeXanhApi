using Application.IService;
using Application.SendModels.RoundTopic;
using Application.SendModels.Topic;
using Application.ViewModels.TopicViewModels;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures;

namespace Application.Services;

public class RoundTopicService : IRoundTopicService
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public RoundTopicService(IUnitOfWork unitOfWork, IMapper mapper, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    #region Delete Topic In Round

    public async Task<bool> DeleteTopicInRound(RoundTopicDeleteRequest roundTopicDeleteRequest)
    {
        var roundtopic =
            await _unitOfWork.RoundTopicRepo.GetByRoundIdTopicId(roundTopicDeleteRequest.RoundId,
                roundTopicDeleteRequest.TopicId);
        if (roundtopic == null) throw new Exception("Khong tim thay RoundTopic");
        await _unitOfWork.RoundTopicRepo.DeleteAsync(roundtopic);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<List<RoundTopicViewModel>> GetListRoundTopicForStaff(Guid id)
    {
        var list = await _unitOfWork.RoundTopicRepo.ListRoundTopicByRoundId(id);
        return _mapper.Map<List<RoundTopicViewModel>>(list);
    }

    #endregion

    #region Add Topic To Round

    public async Task<List<RoundTopicViewModel>> GetListRoundTopicForCompetitor(GetListRoundTopicRequest request)
    {
        var competitor = await _unitOfWork.AccountRepo.GetByIdAsync(request.AccountId);

        var yearOld = DateTime.Today.Year - competitor.Birthday.Value.Year;

        var list = new List<RoundTopic>();

        if (2 <= yearOld && yearOld <= 5)
        {
            var contest = await _unitOfWork.ContestRepo.GetContestByIdForRoundTopic(request.ContestId);
            var education = contest!.EducationalLevel.FirstOrDefault(e => e.Level.Equals("Bảng A"));
            var round = education.Round.FirstOrDefault(r => r.Name.Equals("Vòng Sơ Khảo"));
            list = round.RoundTopic.ToList();
        }
        else if(6 <= yearOld && yearOld <= 11)
        {
            var contest = await _unitOfWork.ContestRepo.GetContestByIdForRoundTopic(request.ContestId);
            var education = contest!.EducationalLevel.FirstOrDefault(e => e.Level.Equals("Bảng B"));
            var round = education.Round.FirstOrDefault(r => r.Name.Equals("Vòng Sơ Khảo"));
            list = round.RoundTopic.ToList();
        }
        else
        {
            throw new Exception("Độ Tuổi Của Bạn Không Hợp Lệ");
        }
        return _mapper.Map<List<RoundTopicViewModel>>(list);
    }

    public async Task<bool> AddTopicToRound(RoundTopicRequest roundTopicRequest)
    {
        var listRoundTopic = new List<RoundTopic>();
        foreach (var topic in roundTopicRequest.ListTopicId)
        {
            var roundtopic = new RoundTopic();
            roundtopic.TopicId = topic;
            roundtopic.RoundId = roundTopicRequest.RoundId;
            listRoundTopic.Add(roundtopic);
        }

        await _unitOfWork.RoundTopicRepo.AddRangeAsync(listRoundTopic);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.RoundTopicRepo.IsExistIdAsync(id);
    }

    #region Validate
    public async Task<ValidationResult> ValidateRoundTopicRequest(RoundTopicRequest roundtopic)
    {
        return await _validatorFactory.RoundTopicRequestValidator.ValidateAsync(roundtopic);
    }

    public async Task<ValidationResult> ValidateRoundTopicDeleteRequest(RoundTopicDeleteRequest roundtopicDelete)
    {
        return await _validatorFactory.RoundTopicDeleteRequestValidator.ValidateAsync(roundtopicDelete);
    }
    #endregion
}