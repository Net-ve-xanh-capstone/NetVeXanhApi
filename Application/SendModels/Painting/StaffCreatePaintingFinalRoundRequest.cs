namespace Application.SendModels.Painting;

public class StaffCreatePaintingFinalRoundRequest
{
    public Guid CompetitorId { get; set; }
    public Guid CurrentUserId { get; set; }
    public string Image { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; } = "Không có mô tả";
    public Guid RoundTopicId { get; set; }
}