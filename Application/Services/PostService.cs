﻿using Application.BaseModels;
using Application.IService;
using Application.SendModels.Post;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures;
using Infracstructures.ViewModels.PostViewModels;

namespace Application.Services;

public class PostService : IPostService
{
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
    {
        _imageService = imageService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Create

    public async Task<bool> CreatePost(PostRequest post)
    {
        var newPost = _mapper.Map<Post>(post);
        var newImages = _mapper.Map<List<Image>>(post.Images);
        newPost.Images = newImages;
        newPost.Status = PostStatus.Active.ToString();
        await _unitOfWork.PostRepo.AddAsync(newPost);
        return await _unitOfWork.SaveChangesAsync()>0;
    }

    #endregion

    #region Get All

    public async Task<(List<PostViewModel>, int)> GetListPost(ListModels listModels)
    {
        var list = await _unitOfWork.PostRepo.GetAllAsync();
        list = (List<Post>)list.Where(x => x.Status == PostStatus.Active.ToString()).OrderByDescending(x => x.CreatedTime);

        var result = new List<Post>();

        //page division
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (_mapper.Map<List<PostViewModel>>(result), totalPages);
    }

    public async Task<List<PostViewModel>> Get10Post()
    {
        var list = await _unitOfWork.PostRepo.Get10Post();
        return _mapper.Map<List<PostViewModel>>(list);
    }

    #endregion

    #region Get By Id

    public async Task<PostViewModel?> GetPostById(Guid id)
    {
        var Post = await _unitOfWork.PostRepo.GetByIdAsync(id);
        if (Post == null) return null;
        return _mapper.Map<PostViewModel>(Post);
    }

    #endregion

    #region Update

    public async Task<bool> UpdatePost(PostUpdateRequest updatePost)
    {
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
                post.Images.FirstOrDefault(img => img.Id == image)!.Status = ImageStatus.Inactive.ToString();

        return await _unitOfWork.SaveChangesAsync()>0;
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

    #region Get 10 Post

    public async Task<(List<PostViewModel>, int)> GetList10Post(ListModels listModels)
    {
        var list = await _unitOfWork.PostRepo.GetAllAsync();
        list = (List<Post>)list.Where(x => x.Status == PostStatus.Active.ToString()).OrderByDescending(x => x.CreatedTime).Take(10);

        var result = new List<Post>();

        #region  Pagination
        var totalPages = (int)Math.Ceiling((double)list.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        #endregion

        return (_mapper.Map<List<PostViewModel>>(result), totalPages);
    }

    #endregion
}