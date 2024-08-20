using Application.BaseModels;
using Application.SendModels.Post;
using Application.ViewModels.PostViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IPostService
{
    public Task<bool> CreatePost(PostRequest Post);
    public Task<(List<ListPostResponse>, int)> GetListPost(ListModels listModels);
    public Task<List<PostResponse>> Get10Post();
    public Task<PostResponse?> GetPostById(Guid id);
    public Task<bool> UpdatePost(PostUpdateRequest updatePost);
    public Task<bool> DeletePost(Guid id);
    Task<(List<PostResponse>, int)> GetPosByStaffId(ListModels listModels, Guid staffId);
    Task<(List<PostResponse>, int)> ListPostByCategoryId(ListModels listPostModel, Guid categoryId);

    Task<(List<PostResponse>, int)> SearchByTitleDescription(ListModels listModels, string searchString);
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidatePostRequest(PostRequest post);

    Task<ValidationResult> ValidatePostUpdateRequest(PostUpdateRequest postUpdate);
}