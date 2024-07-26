namespace Application.ViewModels.ContestViewModels;

public class ContestViewModel
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdateBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public string Status { get; set; }
    public DateTime EndTime { get; set; }
    public string Content { get; set; }
    public string Logo { get; set; }

    public Guid? StaffId { get; set; }
    public string AccountFullName { get; set; }

    //public ICollection<EducationalLevelViewModel> EducationalLevelViewModels { get; set;}
}