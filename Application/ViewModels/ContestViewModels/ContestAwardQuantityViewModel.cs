namespace Application.ViewModels.ContestViewModels;

public class ContestAwardQuantityViewModel
{
    public int Year { get; set; }
    public List<AwardQuanity> AwardQuanity { get; set; } = null!;
}

public class AwardQuanity
{
    public string Name { get; set; } = null!;
    public int Quantity;
}