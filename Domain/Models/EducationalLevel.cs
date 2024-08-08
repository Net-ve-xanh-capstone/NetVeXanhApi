using Domain.Models.Base;

namespace Domain.Models;

public class EducationalLevel : BaseModel
{
    public string? Description { get; set; }
    public string? Level { get; set; }
    public Guid? ContestId { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }

    //Relation
    public ICollection<Award> Award { get; set; }
    public Contest Contest { get; set; }
    public ICollection<Round> Round { get; set; }
}