namespace Application.ViewModels.PostViewModels;

public class ListPostResponse
{
    public Guid Id { get; set; }
    public string? Url { get; set; }
    public string Content { get; set; }
    public string? Image { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? StaffId { get; set; }
    public Guid? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    //Get Image
    public List<ImageInPostVM> Images { get; set; } = new();
}

public class ImageInPostVM
{
    public Guid Id { get; set; }
    public string Url { get; set; }
}