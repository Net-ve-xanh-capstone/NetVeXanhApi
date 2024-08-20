namespace Application.ViewModels.CategoryViewModels;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? UpdatedTime { get; set; }
}