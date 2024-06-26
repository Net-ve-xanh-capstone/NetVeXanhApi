namespace Application.SendModels.Painting;

public class RatingPreliminaryRound
{
    public Guid ScheduleId { get; set; }
    public List<Guid> Paintings { get; set; }
}