using Domain.Models;

namespace Application.IRepositories;

public interface IRoundTopicRepository : IGenericRepository<RoundTopic>
{
    Task<List<Painting>?> ListPaintingForQualifyingRound(Guid roundId, int number);
    Task<List<RoundTopic>> ListRoundTopicByRoundId(Guid roundId);
    Task<List<Painting>> ListPaintingForFinalRound(Guid roundId, int number);
    Task<Guid?> GetRoundTopicId(Guid roundId, Guid topicId);
    Task<RoundTopic?> GetByRoundIdTopicId(Guid roundId, Guid topicId);
}