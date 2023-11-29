using lab5.EF;
using lab5.Models;
using lab5.Models.ApartmentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace lab5.Controllers
{
    public class ApartmentController : BaseController
    {
        private readonly string _key = "apartments";
        public ApartmentController(UtilitiesContext context, IMemoryCache cache) : base(context, cache)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string sortType = "id_asc")
        {
            var paginationModel = new PaginationModel(_context.Apartments.Count(), pageNumber, pageSize);
            var filtrationModel = GetFilterModel();
            IEnumerable<Apartment> apartments;

            if (!_cache.TryGetValue(_key, out apartments))
            {
                apartments = GetApartmentPage(paginationModel, filtrationModel, sortType); ;
                _cache.Set(_key, apartments);
            }
            var viewModel = new ApartmentIndexModel()
            {
                PaginationModel = paginationModel,
                Apartments = apartments,
                SortType = sortType,
                FilterModel = filtrationModel
            };
            return View(viewModel);
        }

        private IQueryable<Apartment> GetApartmentPage(PaginationModel paginationModel, ApartmentFiltrationModel filtrationModel, string sortType)
        {
            var apartments = _context.Apartments.Where(a => a.FirstName.Contains(filtrationModel.FirstName) &&
                                                       a.LastName.Contains(filtrationModel.LastName) &&
                                                       a.MiddleName.Contains(filtrationModel.MiddleName));

            switch (sortType)
            {
                case "id_asc":
                    apartments = apartments.OrderBy(c => c.Id);
                    break;
                case "id_desc":
                    apartments = apartments.OrderByDescending(c => c.Id);
                    break;
                case "firstname_asc":
                    apartments = apartments.OrderBy(c => c.FirstName);
                    break;
                case "firstname_desc":
                    apartments = apartments.OrderByDescending(c => c.FirstName);
                    break;
                case "lastname_asc":
                    apartments = apartments.OrderBy(c => c.LastName);
                    break;
                case "lastname_desc":
                    apartments = apartments.OrderByDescending(c => c.LastName);
                    break;
                case "middlename_asc":
                    apartments = apartments.OrderBy(c => c.MiddleName);
                    break;
                case "middlename_desc":
                    apartments = apartments.OrderByDescending(u => u.MiddleName);
                    break;
                case "humancount_asc":
                    apartments = apartments.OrderBy(c => c.HumanCount);
                    break;
                case "humancount_desc":
                    apartments = apartments.OrderByDescending(c => c.HumanCount);
                    break;
                case "square_asc":
                    apartments = apartments.OrderBy(c => c.Square);
                    break;
                case "square_desc":
                    apartments = apartments.OrderByDescending(c => c.Square);
                    break;
                default:
                    apartments.OrderBy(c => c.Id);
                    break;
            }

            int totalPages = (int)Math.Ceiling(apartments.Count() / (double)paginationModel.PageSize);
            if (paginationModel.PageNumber > totalPages)
                paginationModel.PageNumber = 1;

            return apartments.Skip((paginationModel.PageNumber - 1) * paginationModel.PageSize).Take(paginationModel.PageSize);
        }

        private ApartmentFiltrationModel GetFilterModel()
        {
            string firstname = GetStringFromSession(HttpContext, "firstname", "firstName");
            HttpContext.Session.SetString("firstname", firstname);
            string lastname = GetStringFromSession(HttpContext, "lastname", "lastName");
            HttpContext.Session.SetString("lastname", lastname);
            string middlename = GetStringFromSession(HttpContext, "middlename", "middleName");
            HttpContext.Session.SetString("middlename", middlename);
            var filterModel = new ApartmentFiltrationModel()
            {
                FirstName = firstname,
                LastName = lastname,
                MiddleName = middlename
            };

            return filterModel;
        }
        [Authorize(Roles = "admin")]
        public IActionResult CreateView()
        {
            return View("Create");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult UpdateView(int id)
        {
            var apartment = _context.Apartments.FirstOrDefault(x => x.Id == id);
            return View("Update", apartment);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Update(Apartment apartment)
        {
            _context.Apartments.Update(apartment);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var apartment = _context.Apartments.FirstOrDefault(x => x.Id == id);
            _context.Apartments.Remove(apartment);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
    }
}
