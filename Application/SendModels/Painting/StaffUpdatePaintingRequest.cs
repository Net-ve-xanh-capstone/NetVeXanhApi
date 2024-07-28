namespace Application.SendModels.Painting;

public class StaffUpdatePaintingRequest
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTime Birthday { get; set; }
    
    public string? Image { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public Guid? RoundTopicId { get; set; }
    public Guid CurrentUserId { get; set; }
}