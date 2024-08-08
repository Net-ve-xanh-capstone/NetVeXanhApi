namespace Infracstructures.SendModels.Painting;

public class PaintingUpdateStatusRequest
{
    public Guid Id { get; set; }
    public string? Reason { get; set; } = "Không có thông tin";
    public bool IsPassed { get; set; }
}