﻿using Application.BaseModels;
using Application.SendModels.Resources;
using Application.ViewModels.ResourcesViewModels;

namespace Application.IService;

public interface IResourcesService
{
    public Task<bool> CreateResources(ResourcesRequest Resources);
    public Task<(List<ResourcesViewModel>, int)> GetListResources(ListModels listModels);
    public Task<ResourcesViewModel?> GetResourcesById(Guid id);
    public Task<bool> UpdateResources(ResourcesUpdateRequest updateResources);
    public Task<bool> DeleteResources(Guid id);
}