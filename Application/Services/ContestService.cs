using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Contest;
using Application.SendModels.Topic;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation.Results;
using Infracstructures;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ContestService : IContestService
{
    private readonly IClaimsService _claimsService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentTime _currentTime;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public ContestService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService, IValidatorFactory validatorFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
        _validatorFactory = validatorFactory;
    }

    #region Add Contest

    public async Task<bool> AddContest(ContestRequest addContestViewModel)
    {
        #region Tạo Contest

        var contest = _mapper.Map<Contest>(addContestViewModel);

        /*if (await _unitOfWork.ContestRepo.CheckContestExist(contest.StartTime))
            throw new Exception("Đã Tồn Tại Cuộc Thi Cho Năm Nay");*/

        contest.Status = ContestStatus.NotStarted.ToString();
        contest.CreatedTime = _currentTime.GetCurrentTime();

        // Create levels
        var listLevel = new List<EducationalLevel>();

        var level = new EducationalLevel
        {
            Level = "Bảng A",
            CreatedBy = addContestViewModel.CurrentUserId,
            Contest = contest,
            Status = EducationalLevelStatus.NotStarted.ToString(),
            CreatedTime = _currentTime.GetCurrentTime(),
            Description = "Mầm Non"
        };
        listLevel.Add(level);

        var level2 = new EducationalLevel
        {
            Level = "Bảng B",
            CreatedBy = addContestViewModel.CurrentUserId,
            Contest = contest,
            Status = EducationalLevelStatus.NotStarted.ToString(),
            CreatedTime = _currentTime.GetCurrentTime(),
            Description = "Tiểu Học"
        };
        listLevel.Add(level2);

        // Add levels to contest
        contest.EducationalLevel = listLevel;

        // Create rounds
        var listRound = new List<Round>();

        var round = new Round
        {
            Name = "Vòng Sơ Khảo",
            CreatedBy = addContestViewModel.CurrentUserId,
            EducationalLevel = level,
            Status = RoundStatus.NotStarted.ToString(),
            CreatedTime = _currentTime.GetCurrentTime(),
            StartTime = addContestViewModel.Round1StartTime,
            EndTime = addContestViewModel.Round1EndTime,
            Description = "Không có mô tả",
            Location = "Chưa có thông tin địa điểm"
        };
        listRound.Add(round);

        var round2 = new Round
        {
            Name = "Vòng Chung Kết",
            CreatedBy = addContestViewModel.CurrentUserId,
            EducationalLevel = level,
            Status = RoundStatus.NotStarted.ToString(),
            CreatedTime = _currentTime.GetCurrentTime(),
            StartTime = addContestViewModel.Round2StartTime,
            EndTime = addContestViewModel.Round2EndTime,
            Description = "Không có mô tả",
            Location = "Chưa có thông tin địa điểm"
        };
        listRound.Add(round2);

        var round3 = new Round
        {
            Name = "Vòng Sơ Khảo",
            CreatedBy = addContestViewModel.CurrentUserId,
            EducationalLevel = level2,
            Status = RoundStatus.NotStarted.ToString(),
            CreatedTime = _currentTime.GetCurrentTime(),
            StartTime = addContestViewModel.Round1StartTime,
            EndTime = addContestViewModel.Round1EndTime,
            Description = "Không có mô tả",
            Location = "Chưa có thông tin địa điểm"
        };
        listRound.Add(round3);

        var round4 = new Round
        {
            Name = "Vòng Chung Kết",
            CreatedBy = addContestViewModel.CurrentUserId,
            EducationalLevel = level2,
            Status = RoundStatus.NotStarted.ToString(),
            CreatedTime = _currentTime.GetCurrentTime(),
            StartTime = addContestViewModel.Round2StartTime,
            EndTime = addContestViewModel.Round2EndTime,
            Description = "Không có mô tả",
            Location = "Chưa có thông tin địa điểm"
        };
        listRound.Add(round4);

        // Add rounds to levels
        level.Round = listRound.Where(r => r.EducationalLevel == level).ToList();
        level2.Round = listRound.Where(r => r.EducationalLevel == level2).ToList();

        // Create awards
        var listAward = new List<Award>();

        // Create awards for Level 1
        var awardsLevel1 = new List<Award>
{
    new Award
    {
        Rank = "FirstPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank1,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level
    },
    new Award
    {
        Rank = "SecondPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank2,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level
    },
    new Award
    {
        Rank = "ThirdPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank3,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level
    },
    new Award
    {
        Rank = "ConsolationPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank4,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level
    },
    new Award
    {
        Rank = "Preliminary",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.PassRound1,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level
    }
};

        listAward.AddRange(awardsLevel1);

        // Create awards for Level 2
        var awardsLevel2 = new List<Award>
{
    new Award
    {
        Rank = "FirstPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank1,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level2
    },
    new Award
    {
        Rank = "SecondPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank2,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level2
    },
    new Award
    {
        Rank = "ThirdPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank3,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level2
    },
    new Award
    {
        Rank = "ConsolationPrize",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.Rank4,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level2
    },
    new Award
    {
        Rank = "Preliminary",
        CreatedBy = addContestViewModel.CurrentUserId,
        CreatedTime = _currentTime.GetCurrentTime(),
        Quantity = addContestViewModel.PassRound1,
        Status = ContestStatus.NotStarted.ToString(),
        EducationalLevel = level2
    }
};

        listAward.AddRange(awardsLevel2);

        // Add awards to levels
        level.Award = listAward.Where(a => a.EducationalLevel == level).ToList();
        level2.Award = listAward.Where(a => a.EducationalLevel == level2).ToList();

        // Add contest with levels, rounds, and awards
        await _unitOfWork.ContestRepo.AddAsync(contest);

        #endregion

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete Contest

    public async Task<bool> DeleteContest(Guid contestId)
    {
        var contest = await _unitOfWork.ContestRepo.GetByIdAsync(contestId);
        if (contest == null) throw new Exception("Khong tim thay Contest");

        //Contest
        contest.Status = ContestStatus.Delete.ToString();

        //Resource
        foreach (var resource in contest.Resources) resource.Status = ResourcesStatus.Inactive.ToString();

        //Level 
        foreach (var level in contest.EducationalLevel)
        {
            //round
            foreach (var round in level.Round)
            {
                round.Status = RoundStatus.Delete.ToString();
                foreach (var schedule in round.Schedule) schedule.Status = ScheduleStatus.Delete.ToString();
            }

            //award
            foreach (var award in level.Award) award.Status = AwardStatus.Inactive.ToString();

            level.Status = EducationalLevelStatus.Delete.ToString();
        }


        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Update Contest

    public async Task<bool> UpdateContest(UpdateContest updateContest)
    {
        var contest = await _unitOfWork.ContestRepo.GetByIdAsync(updateContest.Id);
        if (contest == null) throw new Exception("Khong tim thay Contest");

        _mapper.Map(updateContest, contest);
        contest.UpdatedTime = _currentTime.GetCurrentTime();


        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get Contest By Id

    public async Task<ContestDetailViewModel> GetContestById(Guid contestId)
    {
        var contest = await _unitOfWork.ContestRepo.GetAllContestInformationAsync(contestId);
        if (contest == null) throw new Exception("Khong tim thay Contest");

        return _mapper.Map<ContestDetailViewModel>(contest);
    }

    #endregion

    #region Get 5 recent contest year

    public async Task<List<int>> Get5RecentYear()
    {
        var result = await _unitOfWork.ContestRepo.Get5RecentYearAsync();
        return result;
    }

    #endregion

    #region Get All Contest

    public async Task<List<ContestViewModel?>> GetAllContest()
    {
        var contest = await _unitOfWork.ContestRepo.GetAllAsync();
        if (contest.Count == 0) throw new Exception("Khong co Contest nao");
        return _mapper.Map<List<ContestViewModel>>(contest);
    }

    #endregion

    #region Get Nearest Contest

    public async Task<ContestDetailViewModel> GetNearestContest()
    {
        var contest = await _unitOfWork.ContestRepo.GetNearestContestInformationAsync();
        if (contest == null) throw new Exception("Không có Contest nào");

        return _mapper.Map<ContestDetailViewModel>(contest);
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.ContestRepo.IsExistIdAsync(id);
    }

    #region Validate
    public async Task<ValidationResult> ValidateContestRequest(ContestRequest contest)
    {
        return await _validatorFactory.ContestRequestValidator.ValidateAsync(contest);
    }

    public async Task<ValidationResult> ValidateContestUpdateRequest(UpdateContest contestUpdate)
    {
        return await _validatorFactory.UpdateContestRequestValidator.ValidateAsync(contestUpdate);
    }
    #endregion
}