using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Contest;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
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

    public ContestService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime,
        IConfiguration configuration, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _claimsService = claimsService;
    }

    #region Add Contest

    public async Task<bool> AddContest(ContestRequest addContestViewModel)
    {
        #region Tạo Contest

        var contest = _mapper.Map<Contest>(addContestViewModel);

        if (await _unitOfWork.ContestRepo.CheckContestExist(contest.StartTime)) throw new Exception("Đã Tồn Tại Cuộc Thi Cho Năm Nay");
        
        contest.Status = ContestStatus.Inactive.ToString();
        contest.CreatedTime = _currentTime.GetCurrentTime();
        await _unitOfWork.ContestRepo.AddAsync(contest);
        var check = await _unitOfWork.SaveChangesAsync() > 0;

        //check
        if (check == false) throw new Exception("Tạo Contest Thất Bại");

        #endregion


        #region Tạo Level

        //List level
        var listLevel = new List<EducationalLevel>();

        //Create Level Mầm Non
        var level = new EducationalLevel();
        level.Level = "Bảng A";
        level.CreatedBy = addContestViewModel.CurrentUserId;
        level.ContestId = contest.Id;
        level.Status = EducationalLevelStatus.Inactive.ToString();
        level.CreatedTime = _currentTime.GetCurrentTime();
        level.Description = "Mầm Non";
        listLevel.Add(level);

        //Create Level Cấp 1
        var level2 = new EducationalLevel();
        level2.Level = "Bảng B";
        level2.CreatedBy = addContestViewModel.CurrentUserId;
        level2.ContestId = contest.Id;
        level2.Status = EducationalLevelStatus.Inactive.ToString();
        level2.CreatedTime = _currentTime.GetCurrentTime();
        level2.Description = "Tiểu Học";
        listLevel.Add(level2);
        await _unitOfWork.EducationalLevelRepo.AddRangeAsync(listLevel);
        check = await _unitOfWork.SaveChangesAsync() > 0;


        //check
        if (check == false) throw new Exception("Tạo Level Thất Bại");

        #endregion


        #region Tạo Round

        //List level
        var listRound = new List<Round>();
        // Create Round 1 Level 1
        var round = new Round();
        round.Name = "Vòng Sơ Khảo";
        round.CreatedBy = addContestViewModel.CurrentUserId;
        round.EducationalLevelId = level.Id;
        round.Status = RoundStatus.Inactive.ToString();
        round.CreatedTime = _currentTime.GetCurrentTime();
        round.StartTime = addContestViewModel.Round1StartTime;
        round.EndTime = addContestViewModel.Round1EndTime;
        round.Description = "Không có mô tả";
        round.Location = "Chưa có thông tin địa điểm";
        listRound.Add(round);

        // Create Round 2 Level 1
        var round2 = new Round();
        round2.Name = "Vòng Chung Kết";
        round2.CreatedBy = addContestViewModel.CurrentUserId;
        round2.EducationalLevelId = level.Id;
        round2.Status = RoundStatus.Inactive.ToString();
        round2.CreatedTime = _currentTime.GetCurrentTime();
        round2.StartTime = addContestViewModel.Round2StartTime;
        round2.EndTime = addContestViewModel.Round2EndTime;
        round2.Description = "Không có mô tả";
        round2.Location = "Chưa có thông tin địa điểm";
        listRound.Add(round2);

        // Create Round 1 Level 2
        var round3 = new Round();
        round3.Name = "Vòng Sơ Khảo";
        round3.CreatedBy = addContestViewModel.CurrentUserId;
        round3.EducationalLevelId = level2.Id;
        round3.Status = RoundStatus.Inactive.ToString();
        round3.CreatedTime = _currentTime.GetCurrentTime();
        round3.StartTime = addContestViewModel.Round1StartTime;
        round3.EndTime = addContestViewModel.Round1EndTime;
        round3.Description = "Không có mô tả";
        round3.Location = "Chưa có thông tin địa điểm";
        listRound.Add(round3);

        // Create Round 2 Level 2
        var round4 = new Round();
        round4.Name = "Vòng Chung Kết";
        round4.CreatedBy = addContestViewModel.CurrentUserId;
        round4.EducationalLevelId = level2.Id;
        round4.Status = RoundStatus.Inactive.ToString();
        round4.CreatedTime = _currentTime.GetCurrentTime();
        round4.StartTime = addContestViewModel.Round2StartTime;
        round4.EndTime = addContestViewModel.Round2EndTime;
        round4.Description = "Không có mô tả";
        round4.Location = "Chưa có thông tin địa điểm";
        listRound.Add(round4);
        await _unitOfWork.RoundRepo.AddRangeAsync(listRound);
        check = await _unitOfWork.SaveChangesAsync() > 0;

        //check
        if (check == false) throw new Exception("Tạo Round Thất Bại");

        #endregion


        #region Tạo Award

        //List level
        var listAward = new List<Award>();

        //Create 1st prize Level 1
        var award1 = new Award();
        award1.Rank = "FirstPrize";
        award1.CreatedBy = addContestViewModel.CurrentUserId;
        award1.CreatedTime = _currentTime.GetCurrentTime();
        award1.Quantity = addContestViewModel.Rank1;
        award1.Status = ContestStatus.Active.ToString();
        award1.EducationalLevelId = level.Id;
        listAward.Add(award1);

        //Create 2nd prize  Level 1
        var award2 = new Award();
        award2.Rank = "SecondPrize";
        award2.CreatedBy = addContestViewModel.CurrentUserId;
        award2.CreatedTime = _currentTime.GetCurrentTime();
        award2.Quantity = addContestViewModel.Rank2;
        award2.Status = ContestStatus.Active.ToString();
        award2.EducationalLevelId = level.Id;
        listAward.Add(award2);

        //Create 3rd prize Level 1
        var award3 = new Award();
        award3.Rank = "ThirdPrize";
        award3.CreatedBy = addContestViewModel.CurrentUserId;
        award3.CreatedTime = _currentTime.GetCurrentTime();
        award3.Quantity = addContestViewModel.Rank3;
        award3.Status = ContestStatus.Active.ToString();
        award3.EducationalLevelId = level.Id;
        listAward.Add(award3);

        //Create 4th prize Level 1
        var award4 = new Award();
        award4.Rank = "ConsolationPrize";
        award4.CreatedBy = addContestViewModel.CurrentUserId;
        award4.CreatedTime = _currentTime.GetCurrentTime();
        award4.Quantity = addContestViewModel.Rank4;
        award4.Status = ContestStatus.Active.ToString();
        award4.EducationalLevelId = level.Id;
        listAward.Add(award4);

        //Create Passed Level 1
        var award9 = new Award();
        award9.Rank = "Preliminary";
        award9.CreatedBy = addContestViewModel.CurrentUserId;
        award9.CreatedTime = _currentTime.GetCurrentTime();
        award9.Quantity = addContestViewModel.PassRound1;
        award9.Status = ContestStatus.Active.ToString();
        award9.EducationalLevelId = level.Id;
        listAward.Add(award9);

        //Create 1st prize Level 2
        var award5 = new Award();
        award5.Rank = "FirstPrize";
        award5.CreatedBy = addContestViewModel.CurrentUserId;
        award5.CreatedTime = _currentTime.GetCurrentTime();
        award5.Quantity = addContestViewModel.Rank1;
        award5.Status = ContestStatus.Active.ToString();
        award5.EducationalLevelId = level2.Id;
        listAward.Add(award5);

        //Create 2nd prize  Level 2
        var award6 = new Award();
        award6.Rank = "SecondPrize";
        award6.CreatedBy = addContestViewModel.CurrentUserId;
        award6.CreatedTime = _currentTime.GetCurrentTime();
        award6.Quantity = addContestViewModel.Rank2;
        award6.Status = ContestStatus.Active.ToString();
        award6.EducationalLevelId = level2.Id;
        listAward.Add(award6);

        //Create 3rd prize Level 2
        var award7 = new Award();
        award7.Rank = "ThirdPrize";
        award7.CreatedBy = addContestViewModel.CurrentUserId;
        award7.CreatedTime = _currentTime.GetCurrentTime();
        award7.Quantity = addContestViewModel.Rank3;
        award7.Status = ContestStatus.Active.ToString();
        award7.EducationalLevelId = level2.Id;
        listAward.Add(award7);

        //Create 4th prize Level 2
        var award8 = new Award();
        award8.Rank = "ConsolationPrize";
        award8.CreatedBy = addContestViewModel.CurrentUserId;
        award8.CreatedTime = _currentTime.GetCurrentTime();
        award8.Quantity = addContestViewModel.Rank4;
        award8.Status = ContestStatus.Active.ToString();
        award8.EducationalLevelId = level2.Id;
        listAward.Add(award8);

        //Create Passed Level 2
        var award10 = new Award();
        award10.Rank = "Preliminary";
        award10.CreatedBy = addContestViewModel.CurrentUserId;
        award10.CreatedTime = _currentTime.GetCurrentTime();
        award10.Quantity = addContestViewModel.PassRound1;
        award10.Status = ContestStatus.Active.ToString();
        award10.EducationalLevelId = level.Id;
        listAward.Add(award10);

        await _unitOfWork.AwardRepo.AddRangeAsync(listAward);
        check = await _unitOfWork.SaveChangesAsync() > 0;
        //check
        if (check == false) throw new Exception("Tạo Level Thất Bại");

        #endregion

        return check;
    }

    #endregion

    #region Delete Contest

    public async Task<bool> DeleteContest(Guid contestId)
    {
        var contest = await _unitOfWork.ContestRepo.GetByIdAsync(contestId);
        if (contest == null) throw new Exception("Khong tim thay Contest");

        //Contest
        contest.Status = ContestStatus.Inactive.ToString();

        //Resource
        foreach (var resource in contest.Resources) resource.Status = ResourcesStatus.Inactive.ToString();

        //Level 
        foreach (var level in contest.EducationalLevel)
        {
            //round
            foreach (var round in level.Round)
            {
                round.Status = RoundStatus.Inactive.ToString();
                foreach (var schedule in round.Schedule) schedule.Status = ScheduleStatus.Delete.ToString();
            }

            //award
            foreach (var award in level.Award) award.Status = AwardStatus.Inactive.ToString();

            level.Status = EducationalLevelStatus.Inactive.ToString();
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
}