using lab5.EF;

namespace lab5.Models.ApartmentModels
{
    public class ApartmentIndexModel
    {
        public PaginationModel PaginationModel { get; set; }
        public ApartmentFiltrationModel FilterModel { get; set; }
        public IEnumerable<Apartment>? Apartments { get; set; }
        public string SortType { get; set; }
    }
}
