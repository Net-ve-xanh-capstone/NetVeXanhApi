using Application.BaseModels;
using Application.SendModels.Post;
using Application.ViewModels.PostViewModels;
using FluentValidation;
using FluentValidation.Results;

namespace Application.IService;

public interface IPostService
{
    public Task<bool> CreatePost(PostRequest Post);
    public Task<(List<ListPostViewModel>, int)> GetListPost(ListModels listModels);
    public Task<List<PostViewModel>> Get10Post();
    public Task<PostViewModel?> GetPostById(Guid id);
    public Task<bool> UpdatePost(PostUpdateRequest updatePost);
    public Task<bool> DeletePost(Guid id);
    Task<(List<PostViewModel>, int)> GetPosByStaffId(ListModels listModels, Guid staffId);
    Task<(List<PostViewModel>, int)> ListPostByCategoryId(ListModels listPostModel, Guid categoryId);

    Task<(List<PostViewModel>, int)> SearchByTitleDescription(ListModels listModels, string searchString);
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidatePostRequest(PostRequest post);

    Task<ValidationResult> ValidatePostUpdateRequest(PostUpdateRequest postUpdate);
}