using Application.ViewModels.ScheduleViewModels;

namespace Application.IService;

public interface IAwardScheduleService
{
    public Task<List<AwardScheduleResponse>> GetListByScheduleId(Guid id);
    public Task<AwardScheduleResponse> GetById(Guid id);
    Task<bool> IsExistedId(Guid id);
}