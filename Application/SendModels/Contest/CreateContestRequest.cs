using Application.SendModels.EducationalLevel;

namespace Application.SendModels.Contest;

public class CreateContestRequest
{
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Content { get; set; }
    public Guid? CreatedBy { get; set; }
    public List<CreateEducationalLevelRequest> EducationalLevel { get; set; } = null!;
}