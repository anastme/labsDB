namespace lab5.EF;

public partial class Apartment
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public int? HumanCount { get; set; }

    public double Square { get; set; }

    public virtual ICollection<Indication> Indications { get; set; } = new List<Indication>();
}
