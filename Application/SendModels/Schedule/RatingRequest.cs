namespace Application.SendModels.Schedule;

public class RatingRequest
{
    public Guid ScheduleId { get; set; }
    public List<PaintingRatingViewModel> Paintings { get; set; }
}
public class PaintingRatingViewModel
{
    public Guid PaintingId { get; set; }
    public bool IsPass { get; set; }
    public string? Reason { get; set; }
}