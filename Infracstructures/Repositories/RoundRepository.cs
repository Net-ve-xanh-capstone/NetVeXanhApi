﻿using Application.IRepositories;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories;

public class RoundRepository : GenericRepository<Round>, IRoundRepository
{
    public RoundRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Round?> GetByIdAsync(Guid id)
    {
        return await DbSet.Include(src => src.Schedule)
            .Include(r => r.EducationalLevel)
            .FirstOrDefaultAsync(src => src.Id == id && src.Status == RoundStatus.Active.ToString());
    }

    public override async Task<List<Round>> GetAllAsync()
    {
        return await DbSet.Include(r => r.EducationalLevel).Where(src => src.Status == RoundStatus.Active.ToString())
            .ToListAsync();
    }
    

    public async Task<Round?> GetRoundDetail(Guid id)
    {
        return await DbSet.Include(r => r.EducationalLevel).ThenInclude(e => e.Award)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Topic>> GetTopic(Guid roundId)
    {
        return await DbSet.Where(src => src.Id == roundId && src.Status == RoundStatus.Active.ToString())
            .SelectMany(src => src.RoundTopic.Select(src => src.Topic).Where(src => src.Status == TopicStatus.Active.ToString()))
            .ToListAsync();
    }

    public async Task<List<Round>> GetRoundByLevelId(Guid levelId)
    {
        return await DbSet.Include(src => src.EducationalLevel).Include(src => src.Schedule).Where(src => src.EducationalLevelId == levelId && src.Status == RoundStatus.Active.ToString())
            .ToListAsync();
    }
    
    public async Task<List<Round>> GetRoundByContestId(Guid id)
    {
        var list = await DbSet.Include(src => src.EducationalLevel).ThenInclude(src => src.Contest).Include(src => src.Schedule).Where(src => src.EducationalLevel.ContestId == id && src.Status == RoundStatus.Active.ToString())
            .ToListAsync();
        return list;
    }
    
    public async Task<List<Round>>? GetRoundsOfThisYear()
    {
        var list = await DbSet.Include(src => src.EducationalLevel).Include(src => src.Schedule).Where(src => src.StartTime <= DateTime.Now && DateTime.Now <= src.EndTime && src.Status == RoundStatus.Active.ToString()).ToListAsync();
        return list;
    }

    public virtual async Task<bool> CheckSubmitValidDate(Guid? roundId)
    {
        var temp = await DbSet.FirstOrDefaultAsync(src => src.Id == roundId);
        bool check = DateTime.Now <= temp.EndTime && DateTime.Now >= temp.StartTime;
        return check;
    }
    
    
    public async Task<List<Round>> EndRound()
    {
        return await DbSet.Where(src => src.EndTime <= DateTime.Now && src.Status == RoundStatus.Active.ToString()).ToListAsync();

    }

    public async Task<List<Round>> StartRound()
    {
        return await DbSet.Where(src => src.StartTime >= DateTime.Now && src.Status == RoundStatus.Active.ToString()).ToListAsync();
    }
}