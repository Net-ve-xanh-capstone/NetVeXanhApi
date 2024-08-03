namespace Application.SendModels.Schedule;

public class RatingRequest
{
    public Guid ScheduleId { get; set; }
    public List<Guid> Paintings { get; set; }
}