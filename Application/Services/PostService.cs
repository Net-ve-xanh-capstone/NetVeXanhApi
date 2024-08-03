using Application.BaseModels;
using Application.IService;
using Application.SendModels.Post;
using Application.SendModels.Topic;
using Application.ViewModels.PostViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using Infracstructures;

namespace Application.Services;

public class PostService : IPostService
{
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidatorFactory _validatorFactory;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService, IValidatorFactory validatorFactory)
    {
        _imageService = imageService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validatorFactory = validatorFactory;
    }

    #region Create

    public async Task<bool> CreatePost(PostRequest post)
    {
        var validationResult = await ValidatePostRequest(post);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var newPost = _mapper.Map<Post>(post);
        var newImages = _mapper.Map<List<Image>>(post.Images);
        newPost.Images = newImages;
        newPost.Status = PostStatus.Active.ToString();
        await _unitOfWork.PostRepo.AddAsync(newPost);
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(post.CategoryId);
        category.Status = CategoryStatus.Used.ToString();
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get All

    public async Task<(List<ListPostViewModel>, int)> GetListPost(ListModels listModels)
    {
        var list = await _unitOfWork.PostRepo.GetAllAsync();
        if (list.Count == 0) throw new Exception("Khong tim thay Post nao");

        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<ListPostViewModel>>(result), totalPages);
    }

    #endregion

    #region Get 10 Post

    public async Task<List<PostViewModel>> Get10Post()
    {
        var list = await _unitOfWork.PostRepo.Get10Post();
        if (list.Count == 0) throw new Exception("Khong tim thay Post nao");
        return _mapper.Map<List<PostViewModel>>(list);
    }

    #endregion

    #region Get By Id

    public async Task<PostViewModel?> GetPostById(Guid id)
    {
        var Post = await _unitOfWork.PostRepo.GetByIdAsync(id);
        if (Post == null) throw new Exception("Khong tim thay Post");
        var reusult = _mapper.Map<PostViewModel>(Post);
        return reusult;
    }

    #endregion

    #region Get By Staff Id

    public async Task<(List<PostViewModel>, int)> GetPosByStaffId(ListModels listModels, Guid staffId)
    {
        var staff = await _unitOfWork.AccountRepo.GetByIdAsync(staffId);
        if (staff == null) throw new Exception("Khong tim thay Staff");

        var list = await _unitOfWork.PostRepo.GetPostByStaffId(staffId);
        if (list.Count == 0) throw new Exception("Khong tim thay Post nao");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<PostViewModel>>(result), totalPages);
    }

    #endregion

    #region List Post By Category Id

    public async Task<(List<PostViewModel>, int)> ListPostByCategoryId(ListModels listPostModel, Guid categoryId)
    {
        var category = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryId);
        if (category == null) throw new Exception("Khong tim thay Category");

        var listPost = await _unitOfWork.PostRepo.GetPostByCategory(categoryId);
        if (listPost.Count == 0) throw new Exception("Khong co Post nao trong Category");

        var result = _mapper.Map<List<PostViewModel>>(listPost);

        #region pagination

        var totalPages = (int)Math.Ceiling((double)result.Count / listPostModel.PageSize);
        int? itemsToSkip = (listPostModel.PageNumber - 1) * listPostModel.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listPostModel.PageSize)
            .ToList();

        #endregion

        return (result, totalPages);
    }

    #endregion

    #region Update

    public async Task<bool> UpdatePost(PostUpdateRequest updatePost)
    {
        var validationResult = await ValidatePostUpdateRequest(updatePost);
        if (!validationResult.IsValid)
        {
            // Handle validation failure
            throw new ValidationException(validationResult.Errors);
        }
        var post = await _unitOfWork.PostRepo.GetByIdAsync(updatePost.Id);
        if (post == null) throw new Exception("Khong tim thay Post");
        _mapper.Map(updatePost, post);

        if (updatePost.NewImages != null)
        {
            var newImages = _mapper.Map<List<Image>>(updatePost.NewImages);
            foreach (var image in newImages) post.Images.Add(image);
        }

        if (updatePost.DeleteImages != null)
            foreach (var image in updatePost.DeleteImages)
            {
                var deleteImage = post.Images.FirstOrDefault(x => x.Id == image);
                post.Images.Remove(deleteImage);
            }


        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete

    public async Task<bool> DeletePost(Guid id)
    {
        var Post = await _unitOfWork.PostRepo.GetByIdAsync(id);
        if (Post == null) throw new Exception("Khong tim thay Post");

        Post.Status = PostStatus.Inactive.ToString();

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Search By Title Description

    public async Task<(List<PostViewModel>, int)> SearchByTitleDescription(ListModels listModels, string searchString)
    {
        var list = await _unitOfWork.PostRepo.SearchTitleDescription(searchString);
        if (list.Count == 0) throw new Exception("Khong tim thay Post nao");
        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        var result = list.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<PostViewModel>>(result), totalPages);
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.PostRepo.IsExistIdAsync(id);
    }

    #region Validate
    public async Task<ValidationResult> ValidatePostRequest(PostRequest post)
    {
        return await _validatorFactory.PostRequestValidator.ValidateAsync(post);
    }

    public async Task<ValidationResult> ValidatePostUpdateRequest(PostUpdateRequest postUpdate)
    {
        return await _validatorFactory.UpdatePostRequestValidator.ValidateAsync(postUpdate);
    }
    #endregion
}