namespace Application.SendModels.Schedule;

public class RatingSendModel
{
    public Guid ScheduleId { get; set; }
    public List<PaintingRatingViewModel> Paintings { get; set; }
}

public class PaintingRatingViewModel
{
    public Guid PaintingId { get; set; }
    public Guid? AwardId { get; set; }
    public string Reason { get; set; }
}