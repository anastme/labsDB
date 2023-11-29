namespace lab5.EF;

public partial class Counter
{
    public int Id { get; set; }

    public int PaymentTypeId { get; set; }

    public virtual ICollection<Indication> Indications { get; set; } = new List<Indication>();

    public virtual PaymentType PaymentType { get; set; } = null!;
}
