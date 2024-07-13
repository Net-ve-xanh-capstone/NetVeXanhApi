﻿using Application.IService;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Infracstructures;

namespace Application.Services;

public class AwardScheduleService : IAwardScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AwardScheduleService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<AwardScheduleModels>> GetListByScheduleId(Guid id)
    {
        var list = await _unitOfWork.AwardScheduleRepo.GetListByscheduleId(id);
        return list.Count == 0
            ? throw new Exception("Không tìm thấy AwardSchedule")
            : _mapper.Map<List<AwardScheduleModels>>(list);
    }

    public async Task<AwardScheduleModels> GetById(Guid id)
    {
        var awardSchedule = await _unitOfWork.AwardScheduleRepo.GetByIdAsync(id);
        if (awardSchedule == null) throw new Exception("Khong tim thay");
        return _mapper.Map<AwardScheduleModels>(awardSchedule);
    }
}