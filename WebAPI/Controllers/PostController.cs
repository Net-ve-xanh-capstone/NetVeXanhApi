using Application.BaseModels;
using Application.IService;
using Application.SendModels.Post;
using Application.SendModels.Topic;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/posts/")]
public class PostController : Controller
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    #region Create Post

    [HttpPost]
    public async Task<IActionResult> CreatePost(PostRequest post)
    {
        try
        {
            var validationResult = await _postService.ValidatePostRequest(post);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new BaseFailedResponseModel
                {
                    Status = 400,
                    Message = "Validation failed",
                    Result = false,
                    Errors = errors
                };
                return BadRequest(response);
            }
            var result = await _postService.CreatePost(post);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Post Success",
                Result = result
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

    #region Get 10 Post

    [HttpGet("get10post")]
    public async Task<IActionResult> Get10Post()
    {
        try
        {
            var result = await _postService.Get10Post();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Post Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Post By Page

    [HttpGet]
    public async Task<IActionResult> GetPostByPage([FromQuery] ListModels listPostModel)
    {
        try
        {
            var (list, totalPage) = await _postService.GetListPost(listPostModel);
            if (totalPage < listPostModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Post Success",
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
                    List = new List<Post>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region Get Post By Id

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] Guid id)
    {
        try
        {
            var result = await _postService.GetPostById(id);
            if (result == null) return NotFound(new { Success = false, Message = "Post not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Post Success",
                Result = result
            });
        }
        catch (Exception ex)
        {
            return Ok(new BaseFailedResponseModel
            {
                Status = Ok().StatusCode,
                Message = ex.Message,
                Result = false,
                Errors = ex
            });
        }
    }

    #endregion

    #region Update Post

    [HttpPut]
    public async Task<IActionResult> UpdatePost(PostUpdateRequest updatePost)
    {
        try
        {
            var validationResult = await _postService.ValidatePostUpdateRequest(updatePost);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new BaseFailedResponseModel
                {
                    Status = 400,
                    Message = "Validation failed",
                    Result = false,
                    Errors = errors
                };
                return BadRequest(response);
            }
            var result = await _postService.UpdatePost(updatePost);
            if (result == null) return NotFound(new { Success = false, Message = "Post not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Update Successfully"
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

    #region Delete Post

    [HttpPatch]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        try
        {
            var result = await _postService.DeletePost(id);
            if (result == null) return NotFound();
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Delete Successfully"
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

    #region Get Post By StaffId

    [HttpGet("getpostbyStaffId/{id}")]
    public async Task<IActionResult> GetPostByPage([FromQuery] ListModels listPostModel, [FromRoute] Guid id)
    {
        try
        {
            var (list, totalPage) = await _postService.GetPosByStaffId(listPostModel, id);
            if (totalPage < listPostModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Post Success",
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
                    List = new List<Post>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List Post By Category Id

    [HttpGet("getpostbycategory/{id}")]
    public async Task<IActionResult> ListPostByCategoryId([FromRoute] Guid id, [FromQuery] ListModels listCategoryModel)
    {
        try
        {
            var (list, totalPage) = await _postService.ListPostByCategoryId(listCategoryModel, id);
            if (totalPage < listCategoryModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Post Success",
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
                    List = new List<Post>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion

    #region List Post By Category Id

    [HttpGet("search")]
    public async Task<IActionResult> SearchTitleDescription(string searchString,
        [FromQuery] ListModels listCategoryModel)
    {
        try
        {
            var (list, totalPage) = await _postService.SearchByTitleDescription(listCategoryModel, searchString);
            if (totalPage < listCategoryModel.PageNumber)
                return NotFound(new BaseResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = "Over number page"
                });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Get Post Success",
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
                    List = new List<Post>(),
                    TotalPage = 0
                },
                Errors = ex
            });
        }
    }

    #endregion
}