using Domain.Models.Base;

namespace Domain.Models;

public class Round : BaseModel
{
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public Guid? EducationalLevelId { get; set; }
    public int? RoundNumber { get; set; }


    //Relation

    public ICollection<Award> Award { get; set; }
    public EducationalLevel EducationalLevel { get; set; }
    public ICollection<Schedule> Schedule { get; set; }
    public ICollection<RoundTopic> RoundTopic { get; set; }

    public ICollection<RoundJudgingCriteria> RoundJudgingCriteria { get; set; }
}