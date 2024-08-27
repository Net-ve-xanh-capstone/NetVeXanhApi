using Application.BaseModels;
using Application.IService;
using Application.SendModels.Topic;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Authorize(Roles = "Staff")]
[ApiController]
[Route("api/topics/")]
public class TopicController : Controller
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    #region Create Topic

    /// <summary>
    ///     Tạo Topic
    /// </summary>
    /// <param name="topicRequest">Mô tả request gửi.</param>
    /// <response code="200">Tạo thành công.</response>
    /// <response code="400">Validation không thành công.</response>
    [HttpPost]
    public async Task<IActionResult> CreateTopic(TopicRequest topicRequest)
    {
        try
        {
            var result = await _topicService.CreateTopic(topicRequest);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Tạo chủ đề thành công",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var firstErrorMessage = ex.Errors.FirstOrDefault()?.ErrorMessage;
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = firstErrorMessage,
                Result = false
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Topic By Page

    [HttpGet]
    public async Task<IActionResult> GetTopicByPage([FromQuery] ListModels listTopicModel)
    {
        try
        {
            var (list, totalPage) = await _topicService.GetListTopic(listTopicModel);
            if (totalPage < listTopicModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Trang vượt quá số lượng trang cho phép."
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách chủ đè thành công",
                Result = new
                {
                    List = list,
                    TotalPage = totalPage
                }
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new
                {
                    List = new List<Topic>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get All Topic

    [HttpGet("GetAllTopic")]
    public async Task<IActionResult> GetAllTopic()
    {
        try
        {
            var result = await _topicService.GetAllTopic();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy danh sách chủ đề thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = new List<Topic>(),
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Topic By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopicById([FromRoute] Guid id)
    {
        try
        {
            var result = await _topicService.GetTopicById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Không tìm thấy chủ đề" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Lấy chi tiết chủ đề thành công",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = null,
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Topic

    [HttpPut]
    public async Task<IActionResult> UpdateTopic(TopicUpdateRequest updateTopic)
    {
        try
        {
            var result = await _topicService.UpdateTopic(updateTopic);
            if (!result) return NotFound(new { Success = false, Message = "Không tìm thấy chủ đề" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Chỉnh sửa chủ đề thành công"
            });
        }
        catch (ValidationException ex)
        {
            var firstErrorMessage = ex.Errors.FirstOrDefault()?.ErrorMessage;
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = firstErrorMessage,
                Result = false
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Delete Topic

    [HttpPatch]
    public async Task<IActionResult> DeleteTopic(Guid id)
    {
        try
        {
            var result = await _topicService.DeleteTopic(id);
            if (!result) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Xóa chủ đề thành công"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion
}