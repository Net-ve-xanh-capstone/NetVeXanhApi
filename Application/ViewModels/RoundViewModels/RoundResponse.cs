namespace Application.ViewModels.RoundViewModels;

public class RoundResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public Guid? EducationalLevelId { get; set; }
    public string EducationalLevelName { get; set; }
    public Guid? ContestId { get; set; }
    public string ContestName { get; set; }
}