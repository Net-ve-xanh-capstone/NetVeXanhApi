using Application.SendModels.Award;
using Application.SendModels.Contest;

namespace Application.SendModels.Round;

public class CreateRoundSendModel
{
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int RoundNumber { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? EducationalLevelId { get; set; }
    public List<CreateDependentAwardSendModel> Award { get; set; } = null!;
}