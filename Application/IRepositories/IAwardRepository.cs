﻿using Domain.Models;

namespace Application.IRepositories;

public interface IAwardRepository : IGenericRepository<Award>
{
    Task<List<Guid>> GetAwardIdByListLevelId(List<Guid> listLevelId);
}