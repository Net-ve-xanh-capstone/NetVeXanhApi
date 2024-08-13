namespace Application.ViewModels.EducationalLevelViewModels;

public class EducationalLevelViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public Guid? ContestId { get; set; }
    public Guid Createby { get; set; }
    public string Level { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
}