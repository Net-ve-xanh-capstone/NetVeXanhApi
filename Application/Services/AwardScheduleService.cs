using Application.IService;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;

namespace Application.Services;

public class AwardScheduleService : IAwardScheduleService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AwardScheduleService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<AwardScheduleResponse>> GetListByScheduleId(Guid id)
    {
        var list = await _unitOfWork.AwardScheduleRepo.GetListByscheduleId(id);
        return list.Count == 0
            ? throw new Exception("Không tìm thấy AwardSchedule")
            : _mapper.Map<List<AwardScheduleResponse>>(list);
    }

    public async Task<AwardScheduleResponse> GetById(Guid id)
    {
        var awardSchedule = await _unitOfWork.AwardScheduleRepo.GetByIdAsync(id);
        if (awardSchedule == null) throw new Exception("Không tìm thấy ");
        return _mapper.Map<AwardScheduleResponse>(awardSchedule);
    }

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.AwardScheduleRepo.IsExistIdAsync(id);
    }
}