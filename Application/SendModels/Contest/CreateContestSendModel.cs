using Application.SendModels.EducationalLevel;

namespace Application.SendModels.Contest;

public class CreateContestSendModel
{
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Content { get; set; }
    public Guid? CreatedBy { get; set; }
    public List<CreateEducationalLevelSendModel> EducationalLevel { get; set; } = null!;
}