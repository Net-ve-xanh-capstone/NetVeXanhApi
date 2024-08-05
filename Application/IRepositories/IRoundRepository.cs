using Domain.Models;

namespace Application.IRepositories;

public interface IRoundRepository : IGenericRepository<Round>
{
    public Task<Round?> GetRoundDetail(Guid id);
    Task<List<Topic>> GetTopic(Guid roundId);
    Task<List<Round>> GetRoundByLevelId(Guid levelId);
    Task<bool> CheckSubmitValidDate(Guid? roundId);
    Task<List<Round>> GetRoundByContestId(Guid id);

    Task<List<Round>> GetRoundsOfThisYear();

    public Task<List<Round>> EndRound();
    public Task<List<Round>> StartRound();
}