using lab5.EF;

namespace lab5.Models.PaymentTypeModels
{
    public class PaymentTypeIndexModel
    {
        public PaginationModel PaginationModel { get; set; }
        public PaymentTypeFiltrationModel FilterModel { get; set; }
        public IEnumerable<PaymentType>? PaymentTypes { get; set; }
        public string SortType { get; set; }
    }
}
