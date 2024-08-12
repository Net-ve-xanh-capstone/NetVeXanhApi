namespace Application.SendModels.EducationalLevel;

public class EducationalLevelRequest
{
    public string Description { get; set; } = null!;
    public string Level { get; set; } = null!;
    public Guid ContestId { get; set; }
    public Guid CurrentUserId { get; set; }
}