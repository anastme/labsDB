namespace lab3.Utilities.Domain;

public partial class Rate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Price { get; set; }

    public int PaymentTypeId { get; set; }

    public virtual PaymentType PaymentType { get; set; } = null!;
}
