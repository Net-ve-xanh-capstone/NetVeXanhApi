namespace Application.SendModels.Painting;

public class RatingFinalRound
{
    public Guid ScheduleId { get; set; }
    public List<Guid>? First { get; set; }
    public List<Guid>? Second { get; set; }
    public List<Guid>? Thirst { get; set; }
    public List<Guid>? Fours { get; set; }
}