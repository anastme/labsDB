namespace lab3.Utilities.Domain;

public partial class Apartment
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string MiddleName { get; set; }

    public int HumanCount { get; set; }

    public double Square { get; set; }

    public virtual ICollection<Indication> Indications { get; set; } = new List<Indication>();

    public override string ToString()
    {
        return $"{FirstName} {LastName} {MiddleName}\n" +
               $"кол-во людей: {HumanCount} площадь: {Square}";
    }
}
