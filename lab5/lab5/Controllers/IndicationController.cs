using lab5.EF;
using lab5.Models;
using lab5.Models.IndicationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace lab5.Controllers
{
    public class IndicationController : BaseController
    {
        private readonly string _key = "indications";
        public IndicationController(UtilitiesContext context, IMemoryCache cache) : base(context, cache)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, user")]
        public IActionResult Index([FromQuery] int minVolume, [FromQuery] int maxVolume, int pageNumber = 1, int pageSize = 20, string sortType = "id_asc")
        {
            var paginationModel = new PaginationModel(_context.Indications.Count(), pageNumber, pageSize);
            var filtrationModel = GetFilterModel(minVolume, maxVolume);

            IEnumerable<Indication> indications;
            if (!_cache.TryGetValue(_key, out indications))
            {
                indications = GetIndicationPage(paginationModel, filtrationModel, sortType); ;
                _cache.Set(_key, indications);
            }

            var viewModel = new IndicationIndexModel()
            {
                PaginationModel = paginationModel,
                Indications = indications,
                SortType = sortType,
                FilterModel = filtrationModel
            };
            return View(viewModel);
        }

        private IQueryable<Indication> GetIndicationPage(PaginationModel paginationModel, IndicationFiltrationModel filtrationModel, string sortType)
        {
            IQueryable<Indication> indications;
            if (filtrationModel.MinVolume == 0 && filtrationModel.MaxVolume == 0)
                indications = _context.Indications.Include(i => i.Counter).ThenInclude(c => c.PaymentType);
            else
                indications = _context.Indications.Where(a => a.Volume > filtrationModel.MinVolume && a.Volume < filtrationModel.MaxVolume).Include(i => i.Counter).ThenInclude(c => c.PaymentType);

            switch (sortType)
            {
                case "date_asc":
                    indications = indications.OrderBy(c => c.Date);
                    break;
                case "date_desc":
                    indications = indications.OrderByDescending(c => c.Date);
                    break;
                case "volume_asc":
                    indications = indications.OrderBy(c => c.Volume);
                    break;
                case "volume_desc":
                    indications = indications.OrderByDescending(c => c.Volume);
                    break;
                case "apartment_asc":
                    indications = indications.OrderBy(c => c.ApartmentId);
                    break;
                case "apartment_desc":
                    indications = indications.OrderByDescending(u => u.ApartmentId);
                    break;
                case "paymenttype_asc":
                    indications = indications.OrderBy(c => c.Counter.PaymentType.Name);
                    break;
                case "paymenttype_desc":
                    indications = indications.OrderByDescending(c => c.Counter.PaymentType.Name);
                    break;
                default:
                    indications.OrderBy(c => c.ApartmentId);
                    break;
            }

            int totalPages = (int)Math.Ceiling(indications.Count() / (double)paginationModel.PageSize);
            if (paginationModel.PageNumber > totalPages)
                paginationModel.PageNumber = 1;

            return indications.Skip((paginationModel.PageNumber - 1) * paginationModel.PageSize).Take(paginationModel.PageSize);
        }

        private IndicationFiltrationModel GetFilterModel(int minVolume, int maxVolume)
        {
            if (minVolume != 0)
            {
                HttpContext.Session.SetInt32("minVolume", minVolume);
            }
            else
            {
                minVolume = HttpContext.Session.Keys.Contains("minVolume")
                           ? (int)HttpContext.Session.GetInt32("minVolume") : 0;
            }

            if (maxVolume != 0)
            {
                HttpContext.Session.SetInt32("maxVolume", maxVolume);
            }
            else
            {
                maxVolume = HttpContext.Session.Keys.Contains("maxVolume")
                           ? (int)HttpContext.Session.GetInt32("maxVolume") : 0;
            }

            var filterModel = new IndicationFiltrationModel()
            {
                MinVolume = minVolume,
                MaxVolume = maxVolume
            };
            return filterModel;
        }
        [Authorize(Roles = "admin")]
        public IActionResult CreateView()
        {
            var viewModel = new IndicationCreateViewModel()
            {
                CounterId = int.Parse(HttpContext.Request.Query["counterId"][0]),
                Apartments = _context.Apartments.OrderBy(a => a.Id)
            };
            return View("Create", viewModel);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create(Indication indication)
        {
            indication.Date = DateTime.Now;
            _context.Indications.Add(indication);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index", "Counter", new { id = indication.CounterId });
        }
        [Authorize(Roles = "admin")]
        public IActionResult UpdateView(int id)
        {
            var viewModel = new IndicationUpdateViewModel()
            {
                Apartments = _context.Apartments.OrderBy(a => a.Id),
                Indication = _context.Indications.FirstOrDefault(i => i.Id == id)
            };
            return View("Update", viewModel);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Update(Indication indication)
        {
            _context.Indications.Update(indication);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var indication = _context.Indications.FirstOrDefault(i => i.Id == id);
            _context.Indications.Remove(indication);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
    }
}

