using Application.SendModels.Painting;
using Domain.Models;

namespace Application.IRepositories;

public interface IPaintingRepository : IGenericRepository<Painting>
{
    Task<List<Painting>> GetAllPaintingOfRound(Guid id);
    Task<Painting?> GetByCodeAsync(string code);
    Task<List<Painting>> List16WiningPaintingAsync();
    Task<List<Account>> ListCompetitorPassByRound(Guid roundId);
    Task<List<Painting>> ListByAccountIdAsync(Guid accountId);
    Task<List<Guid>> ListAccountIdByListAwardId(List<Guid> listAwardId);
    Task<List<Painting>> FilterPaintingAsync(FilterPaintingRequest filterPainting);
    Task<int> CreateNewNumberOfPaintingCode(Guid? roundId);
    Task<Painting> GetPaintingsByContestAndAccountAsync(Guid contestId, Guid accountId);
    Task<List<Painting>?> GetByScheduleIdAsync(Guid scheduleId);
    Task<int> CountPaintingHaveAward(Guid scheduleId, Guid awardId);

    Task<int> PaintingCountByContest(Guid contestId);
    Task<bool> IsExistPaintingInContest(Guid accountId, Guid roundId);
    Task<Account?> GetAccountByPaintingIdAsync(Guid paintingId);

    Task<int> GetNumPaintingInRound(Guid roundId);
    Task<int> GetNumPaintingInRoundIsHaveSchedule(Guid roundId);
    Task<int> GetNumPaintingInRoundIsNotHaveSchedule(Guid roundId);
}