using Application.SendModels.Contest;
using Application.SendModels.Round;

namespace Application.SendModels.EducationalLevel;

public class CreateEducationalLevelRequest
{
    public string? Level { get; set; }
    public string? Description { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ContestId { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public List<CreateRoundRequest> Round { get; set; } = null!;
}