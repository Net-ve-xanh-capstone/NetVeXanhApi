using Domain.Models;

namespace Application.IRepositories;

public interface IAwardRepository : IGenericRepository<Award>
{
    Task<List<Award>?> GetAwardsByRoundId(Guid roundId);

}