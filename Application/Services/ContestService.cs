﻿using Application.IService;
using Application.ViewModels.AwardViewModels;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Models;
using Infracstructures;
using Microsoft.Extensions.Configuration;
using WebAPI.IService.ICommonService;

namespace Application.Services
{
    public class ContestService : IContestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;

        public ContestService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, IConfiguration configuration, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _configuration = configuration;
            _claimsService = claimsService;
        }

        #region Add Contest
        public async Task<Guid?> AddContest(AddContestViewModel addContestViewModel)
        {
            Contest contest = _mapper.Map<Contest>(addContestViewModel);
            contest.CreatedBy = _claimsService.GetCurrentUserId();
            contest.Status = "ACTIVE";
            await _unitOfWork.ContestRepo.AddAsync(contest);

            var check = await _unitOfWork.SaveChangesAsync() > 0;
            AddContestViewModel result = _mapper.Map<AddContestViewModel>(contest);
            //view.
            if (check)
            {
                return contest.Id;
            }
            return null;
        }
        #endregion

        #region Delete Contest
        public async Task<Guid> DeleteContest(Guid contestId)
        {
            var contest = await _unitOfWork.ContestRepo.GetByIdAsync(contestId);
            if (contest == null)
            {
                return Guid.Empty;
            }
            else
            {
                contest.Status = "INACTIVE";

                await _unitOfWork.SaveChangesAsync();
                return contestId;
            }
        }
        #endregion

        #region Update Contest
        public async Task<UpdateContestViewModel> UpdateContest(UpdateContestViewModel updateContest)
        {
            var contest = await _unitOfWork.ContestRepo.GetByIdAsync(updateContest.Id);
            if (contest == null)
            {
                return null;
            }
            else
            {
                contest.Name = updateContest.Name;
                contest.StartTime = updateContest.StartTime;
                contest.EndTime = updateContest.EndTime;
                contest.Description = updateContest.Description;
                contest.Content = updateContest.Content;
                contest.UpdatedBy = _claimsService.GetCurrentUserId();
                contest.UpdatedTime = _currentTime.GetCurrentTime();


                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<UpdateContestViewModel>(contest);
            }
        }
        #endregion

        #region Get Contest By Id
        public async Task<ContestViewModel> GetContestById(Guid awardId)
        {
            var award = await _unitOfWork.AwardRepo.GetByIdAsync(awardId);

            return _mapper.Map<ContestViewModel>(award); ;
        }
        #endregion

    }
}