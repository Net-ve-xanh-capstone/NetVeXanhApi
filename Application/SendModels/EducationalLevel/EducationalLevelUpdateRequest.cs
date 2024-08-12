namespace Application.SendModels.EducationalLevel;

public class EducationalLevelUpdateRequest
{
    public Guid Id { get; set; }
    public string? Level { get; set; }
    public string? Description { get; set; }
    public Guid? ContestId { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public Guid CurrentUserId { get; set; }
}