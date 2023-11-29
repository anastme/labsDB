using lab5.EF;
using lab5.Models;
using lab5.Models.PaymentTypeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace lab5.Controllers
{
    public class PaymentTypeController : BaseController
    {
        private readonly string _key = "paymentTypes";
        public PaymentTypeController(UtilitiesContext context, IMemoryCache cache) : base(context, cache)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10, string sortType = "name_asc")
        {
            var paginationModel = new PaginationModel(_context.PaymentTypes.Count(), pageNumber, pageSize);
            var filtrationModel = GetFilterModel();
            IEnumerable<PaymentType> paymentTypes;

            if (!_cache.TryGetValue(_key, out paymentTypes))
            {
                paymentTypes = GetPaymentTypePage(paginationModel, filtrationModel, sortType); ;
                _cache.Set(_key, paymentTypes);
            }
            var viewModel = new PaymentTypeIndexModel()
            {
                PaginationModel = paginationModel,
                PaymentTypes = paymentTypes,
                SortType = sortType,
                FilterModel = filtrationModel
            };
            return View(viewModel);
        }
        private IQueryable<PaymentType> GetPaymentTypePage(PaginationModel paginationModel, PaymentTypeFiltrationModel filtrationModel, string sortType)
        {
            var paymentTypes = _context.PaymentTypes.Where(a => a.Name.Contains(filtrationModel.Name));

            switch (sortType)
            {
                case "name_asc":
                    paymentTypes = paymentTypes.OrderBy(c => c.Name);
                    break;
                case "name_desc":
                    paymentTypes = paymentTypes.OrderByDescending(c => c.Name);
                    break;
                default:
                    paymentTypes.OrderBy(c => c.Name);
                    break;
            }

            int totalPages = (int)Math.Ceiling(paymentTypes.Count() / (double)paginationModel.PageSize);
            if (paginationModel.PageNumber > totalPages)
                paginationModel.PageNumber = 1;

            return paymentTypes.Skip((paginationModel.PageNumber - 1) * paginationModel.PageSize).Take(paginationModel.PageSize);
        }
        private PaymentTypeFiltrationModel GetFilterModel()
        {
            string name = GetStringFromSession(HttpContext, "name", "name");
            HttpContext.Session.SetString("name", name);
            var filterModel = new PaymentTypeFiltrationModel()
            {
                Name = name
            };
            return filterModel;
        }
        [Authorize(Roles = "admin")]
        public IActionResult CreateView()
        {
            return View("Create");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create(PaymentType paymentType)
        {
            _context.PaymentTypes.Add(paymentType);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult UpdateView(int id)
        {
            var paymentType = _context.PaymentTypes.FirstOrDefault(p => p.Id == id);
            return View("Update", paymentType);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Update(PaymentType paymentType)
        {
            _context.PaymentTypes.Update(paymentType);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var paymentType = _context.PaymentTypes.FirstOrDefault(p => p.Id == id);
            _context.PaymentTypes.Remove(paymentType);
            _context.SaveChanges();
            CacheClear();
            return RedirectToAction("Index");
        }
    }
}
