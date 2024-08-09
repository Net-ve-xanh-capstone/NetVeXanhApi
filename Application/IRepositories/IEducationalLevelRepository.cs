using Domain.Models;

namespace Application.IRepositories;

public interface IEducationalLevelRepository : IGenericRepository<EducationalLevel>
{
    Task<List<EducationalLevel>> GetEducationalLevelByContestId(Guid contestId);
    Task<List<Guid>> GetLevelIdByListContestId(List<Guid> contestIdList);
}