using Application.SendModels.Contest;
using Application.SendModels.Round;

namespace Application.SendModels.EducationalLevel;

public class CreateEducationalLevelSendModel
{
    public string? Level { get; set; }
    public string? Description { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ContestId { get; set; }
    public List<CreateRoundSendModel> Round { get; set; } = null!;
}