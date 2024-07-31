namespace Application.SendModels.Image;

public class ImageRequest
{
    public string Url { get; set; }
    public string? Description { get; set; } = "Không có mô tả";
}