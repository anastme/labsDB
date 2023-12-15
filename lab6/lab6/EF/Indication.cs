namespace lab6.EF;

public partial class Indication
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double Volume { get; set; }

    public int ApartmentId { get; set; }

    public int CounterId { get; set; }

    public virtual Apartment Apartment { get; set; } = null!;

    public virtual Counter Counter { get; set; } = null!;
}
