namespace Domain.Models.Base;

public class District
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Ward> Wards { get; set; } = null!;
}