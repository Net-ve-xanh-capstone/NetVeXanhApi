namespace Domain.Models.Base;

public class Ward
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid DistrictId { get; set; }
    public District District { get; set; }
}