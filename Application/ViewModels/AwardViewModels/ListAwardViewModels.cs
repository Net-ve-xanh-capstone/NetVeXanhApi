using Domain.Models;

namespace Application.ViewModels.AwardViewModels;

public class ListAwardViewModels
{
    public Guid Id;
    public string Description { get; set; } = null!;
    public string Level { get; set; } = null!;
    public List<AwardViewModel>? AwardViewModels { get; set; }
}