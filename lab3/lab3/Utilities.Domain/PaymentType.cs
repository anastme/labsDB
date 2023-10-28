namespace lab3.Utilities.Domain;

public partial class PaymentType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Counter> Counters { get; set; } = new List<Counter>();

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
