namespace Domain.Models.Base;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime? CreatedTime { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }
    public string Status { get; set; }
    public DateTime? UpdatedTime { get; set; } = DateTime.Now;
    public Guid? UpdatedBy { get; set; }
}