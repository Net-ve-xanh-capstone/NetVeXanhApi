using Domain.Models;

namespace Application.IRepositories;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    public Task<List<Schedule>> GetByExaminerId(Guid id);

    public Task<List<Schedule>> SchedulerTrigger();
    public Task<List<Painting>> GetListByRoundId(Guid roundId);
}