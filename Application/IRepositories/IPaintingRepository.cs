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

    Task<int> PaintingCountByContest(Guid contestId);

    Task<bool> IsExistPaintingInContest(Guid accountId, Guid roundId);
}