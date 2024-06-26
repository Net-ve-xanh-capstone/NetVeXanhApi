using Domain.Models;

namespace Application.IRepositories;

public interface IPaintingRepository : IGenericRepository<Painting>
{
    Task<Painting?> GetByCodeAsync(string code);
    Task<List<Painting>> List20WiningPaintingAsync();

    Task<List<Painting>> ListPaintingForPreliminaryRound(Guid id);
    
    Task<List<Painting>> ListPaintingForFinalRound(Guid id);

    //Task<List<Competitor>> ListCompetitorPassRound(Guid id);
}