using Domain.Models.Base;

namespace Application.IService;

public interface IDistrictService
{
    public Task<List<District>> GetAll();
    public Task<District?> GetById(Guid id);
}