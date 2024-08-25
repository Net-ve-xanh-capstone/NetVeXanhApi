using Application.SendModels.Award;

namespace Application.SendModels.Round;

public class CreateRoundRequest
{
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int RoundNumber { get; set; }
    public Guid? CreatedBy { get; set; }
    public List<Guid>? LevelList { get; set; }
    public List<CreateDependentAwardRequest> Award { get; set; } = null!;
}