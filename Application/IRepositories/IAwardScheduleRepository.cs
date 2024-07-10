﻿using Domain.Models;

namespace Application.IRepositories;

public interface IAwardScheduleRepository : IGenericRepository<AwardSchedule>
{
    public Task<List<AwardSchedule>?> GetListByscheduleId(Guid id);
    
}