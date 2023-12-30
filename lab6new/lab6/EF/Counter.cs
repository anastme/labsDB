﻿using System.Text.Json.Serialization;

namespace lab6.EF;

public partial class Counter
{
    public int Id { get; set; }

    public int PaymentTypeId { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Indication> Indications { get; set; } = new List<Indication>();

    public virtual PaymentType PaymentType { get; set; } = null!;
}
