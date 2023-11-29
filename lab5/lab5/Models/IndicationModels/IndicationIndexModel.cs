using lab5.EF;

namespace lab5.Models.IndicationModels
{
    public class IndicationIndexModel
    {
        public PaginationModel PaginationModel { get; set; }
        public IndicationFiltrationModel FilterModel { get; set; }
        public IEnumerable<Indication>? Indications { get; set; }
        public string SortType { get; set; }
    }
}
