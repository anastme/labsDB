﻿namespace lab6.Models
{
    public class IndicationForUpdate
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double Volume { get; set; }

        public int ApartmentId { get; set; }

        public int PaymentTypeId { get; set; }
    }
}
