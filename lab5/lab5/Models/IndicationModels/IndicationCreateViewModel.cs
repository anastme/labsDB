using lab5.EF;

namespace lab5.Models.IndicationModels
{
    public class IndicationCreateViewModel
    {
        public int CounterId { get; set; }
        public IEnumerable<Apartment> Apartments { get; set; }
    }
}
