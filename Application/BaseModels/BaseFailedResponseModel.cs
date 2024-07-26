namespace Application.BaseModels;

public class 
    BaseFailedResponseModel
{
    public int Status { get; set; }
    public string? Message { get; set; }
    public object? Result { get; set; }
    public object? Errors { get; set; }
}