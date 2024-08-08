using Domain.Models.Base;

namespace Domain.Models;

public class Award : BaseModel
{
    public string? Rank { get; set; }
    public int Quantity { get; set; }
    public double Cash { get; set; }
    public string Artifact { get; set; }
    public string? Description { get; set; }
    public Guid? EducationalLevelId { get; set; }
    public Guid? RoundId { get; set; }

    //Relation
    public Round Round { get; set; }
    public EducationalLevel EducationalLevel { get; set; }
    public ICollection<Painting> Painting { get; set; }

    public ICollection<AwardSchedule> AwardSchedule { get; set; }
}