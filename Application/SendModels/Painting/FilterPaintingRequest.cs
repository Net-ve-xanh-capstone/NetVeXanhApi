namespace Application.SendModels.Painting;

public class FilterPaintingRequest
{
    public string? Code { get; set; }
    public string? TopicName { get; set; }
    public string? ContestId { get; set; }
    public string? Level { get; set; }
    public string? RoundName { get; set; }

    public string? Status { get; set; }
    //public Guid ContestId { get; set; }
}