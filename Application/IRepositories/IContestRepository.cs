﻿using Domain.Models;

namespace Application.IRepositories;

public interface IContestRepository : IGenericRepository<Contest>
{
    Task<Contest> GetAllContestInformationAsync(Guid contestId);
    Task<List<int>> Get5RecentYearAsync();
    Task<bool> CheckValidEducationalLevelDate(Guid ContestId, DateTime EducationalLevelStartTime, DateTime EducationalLevelEndTime);
}