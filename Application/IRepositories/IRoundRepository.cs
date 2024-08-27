using Domain.Models;

namespace Application.IRepositories;

public interface IRoundRepository : IGenericRepository<Round>
{
    Task<Round?> GetRoundDetail(Guid id);
    Task<List<Topic>> GetTopic(Guid roundId);
    Task<List<Round>> GetRoundByLevelId(Guid levelId);
    Task<bool> CheckSubmitValidDate(Guid? roundId);
    Task<List<Round>> GetScheduleByContestId(Guid id);

    Task<List<Round>> GetRoundsOfThisYear();

    Task<List<Round>> EndRound();
    Task<List<Round>> StartRound();
    Task<bool> IsExistNameAsync(string name);
}