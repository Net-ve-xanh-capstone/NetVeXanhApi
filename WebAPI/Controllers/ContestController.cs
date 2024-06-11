﻿using Application.IService;
using Application.ResponseModels;
using Application.Services;
using Application.ViewModels.CollectionViewModels;
using Application.ViewModels.ContestViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContestController : Controller
    {
        private readonly IContestService _contestService;

        public ContestController(IContestService contestService)
        {
            _contestService = contestService;
        }

        #region Create Contest
        [HttpPost()]
        public async Task<IActionResult> CreateContest(AddContestViewModel contest)
        {
            try
            {
                var result = await _contestService.AddContest(contest);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Create Award Success",
                    Result = result
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });

            }
        }
        #endregion

        #region Update Contest
        [HttpPut()]
        public async Task<IActionResult> UpdateContest(UpdateContestViewModel updateContestViewModel)
        {
            var result = await _contestService.UpdateContest(updateContestViewModel);
            if (result == null) return NotFound();
            else
            {
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Result = result,
                    Message = "Update Successfully"
                });
            }
        }
        #endregion

        #region Delete Contest
        [HttpPatch()]
        public async Task<IActionResult> DeleteContest(Guid id)
        {
            var result = await _contestService.DeleteContest(id);
            if (result == null) return NotFound();
            else
            {
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Result = result,
                    Message = "Delete Successfully"
                });
            }
        }
        #endregion

        #region Get Contest By Id
        [HttpGet()]
        public async Task<IActionResult> GetContestById(Guid awardId)
        {
            try
            {
                var result = await _contestService.GetContestById(awardId);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Get Inventory Success",
                    Result = result

                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });
            }
        }
        #endregion
    }
}
