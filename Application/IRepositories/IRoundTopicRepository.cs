using Domain.Models;

namespace Application.IRepositories;

public interface IRoundTopicRepository : IGenericRepository<RoundTopic>
{
    public Task<List<Painting>?> ListPaintingForPreliminaryRound(Guid roundId);
    public Task<List<RoundTopic>> ListRoundTopicByRoundId(Guid roundId);
    public Task<List<Painting>> ListPaintingForFinalRound(Guid roundId);
    Task<Guid?> GetRoundTopicId(Guid roundId, Guid topicId);
    Task<RoundTopic?> GetByRoundIdTopicId(Guid roundId, Guid topicId);
}