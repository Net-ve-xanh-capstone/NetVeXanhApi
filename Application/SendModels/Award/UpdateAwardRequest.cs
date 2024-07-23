namespace Application.SendModels.Award;

public class UpdateAwardRequest
{
    public Guid Id { get; set; }
    public string? Rank { get; set; }
    public int? Quantity { get; set; }
    public double? Cash { get; set; }
    public string? Artifact { get; set; }
    public string? Description { get; set; }
    public Guid? EducationalLevelId { get; set; }
    public Guid CurrentUserId { get; set; }
}