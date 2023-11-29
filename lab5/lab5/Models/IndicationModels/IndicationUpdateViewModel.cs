using lab5.EF;

namespace lab5.Models.IndicationModels
{
    public class IndicationUpdateViewModel
    {
        public IEnumerable<Apartment> Apartments { get; set; }
        public Indication Indication { get; set; }
    }
}
