using Application.BaseModels;
using Application.IService;
using Application.SendModels.Post;
using Domain.Models;
using FluentValidation;
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
            var result = await _postService.CreatePost(post);
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Message = "Create Post Success",
                Result = result
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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
            var result = await _postService.UpdatePost(updatePost);
            if (!result) return NotFound(new { Success = false, Message = "Post not found" });
            return Ok(new BaseResponseModel
            {
                Status = Ok().StatusCode,
                Result = result,
                Message = "Update Successfully"
            });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new BaseFailedResponseModel
            {
                Status = BadRequest().StatusCode,
                Message = "Xác thực không thành công",
                Result = false,
                Errors = errors
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
            if (!result) return NotFound();
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