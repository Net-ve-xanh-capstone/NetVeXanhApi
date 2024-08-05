using Application.IService;
using Domain.Models.Base;

namespace Application.Services;

public class DistrictService : IDistrictService
{
    private readonly IUnitOfWork _unitOfWork;

    public DistrictService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<District>> GetAll()
    {
        return _unitOfWork.DistrictRepo.GetAllAsync();
    }

    public async Task<District?> GetById(Guid id)
    {
        return await _unitOfWork.DistrictRepo.GetByIdAsync(id);
    }
}