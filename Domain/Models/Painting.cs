using Domain.Models.Base;

namespace Domain.Models;

public class Painting : BaseModel
{
    public string Image { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? SubmittedTimestamp { get; set; }
    public DateTime? ReviewedTimestamp { get; set; }
    public DateTime? FinalDecisionTimestamp { get; set; }
    public Guid? AwardId { get; set; }
    public Guid? RoundTopicId { get; set; }
    public Guid AccountId { get; set; }
    public Guid? ScheduleId { get; set; }
    public string Code { get; set; }
    public Guid Reviewer {  get; set; }
    public string? ReviewReason { get; set; }
    public string? JudgementReason { get; set; }






    //Relation

    public ICollection<PaintingCollection> PaintingCollection { get; set; }
    public RoundTopic RoundTopic { get; set; }
    public Award? Award { get; set; }
    public Account Account { get; set; }
    public Schedule? Schedule { get; set; }
}