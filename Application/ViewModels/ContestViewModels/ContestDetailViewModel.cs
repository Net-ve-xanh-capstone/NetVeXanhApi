namespace Application.ViewModels.ContestViewModels;

public class ContestDetailViewModel
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdateBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public string Logo { get; set; }
    public string Description { get; set; }
    public int PaintingCount { get; set; }
    public int CompetitorCount { get; set; }

    public AccountInContestViewModel Account { get; set; }
    public List<ResourceInContestViewModel> Resource { get; set; }
    public List<EducationalLevelInContest> EducationalLevel { get; set; }
}

public class AccountInContestViewModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
}

public class ResourceInContestViewModel
{
    public Guid? Id { get; set; }
    public string Sponsorship { get; set; }
    public SponsorInResourceViewModel Sponsor { get; set; }
}

public class SponsorInResourceViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Delegate { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Logo { get; set; }
}

public class EducationalLevelInContest
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Level { get; set; }
    public List<AwardInLevelViewModel> Award { get; set; }

    public List<RoundInLevelViewModel> Round { get; set; }
}

public class AwardInLevelViewModel
{
    public Guid Id { get; set; }
    public string Rank { get; set; }
    public int Quantity { get; set; }
    public double Cash { get; set; }
    public string Artifact { get; set; }
    public string Description { get; set; }
}

public class RoundInLevelViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<RoundTopicInContestViewModel> RoundTopic { get; set; }
}

public class RoundTopicInContestViewModel
{
    public TopicInRoundViewModel Topic { get; set; }
}

public class TopicInRoundViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}