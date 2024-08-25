namespace Application.ViewModels.ContestViewModels;

public class ContestAwardQuantityResponse
{
    public int Year { get; set; }
    public List<AwardQuanity> AwardQuanity { get; set; } = null!;
}

public class AwardQuanity
{
    public int Quantity;
    public string Name { get; set; } = null!;
}