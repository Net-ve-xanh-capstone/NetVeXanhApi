using Domain.Models;

namespace Application.ViewModels.AwardViewModels;

public class ListAwardResponse
{
    public Guid Id;
    public string Description { get; set; } = null!;
    public string Level { get; set; } = null!;
    public List<AwardViewResponse>? AwardViewModels { get; set; }
}