namespace Application.SendModels.Award;

public class  CreateAwardSendModel
{
    public string Rank { get; set; }
    public int Quantity { get; set; }
    public double Cash { get; set; } = 0;
    public string Artifact { get; set; } = "Không có thông tin";
    public Guid RoundId { get; set; }
    public Guid CreatedBy { get; set; }
}