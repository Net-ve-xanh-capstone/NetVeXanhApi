namespace Application.SendModels.Painting;

public class StaffUpdatePaintingRequest
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    public string Phone { get; set; }
    public DateTime Birthday { get; set; }

    public string Image { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; } = "Không có mô tả";
    public string Status { get; set; }
    public Guid RoundTopicId { get; set; }
    public Guid CurrentUserId { get; set; }
}