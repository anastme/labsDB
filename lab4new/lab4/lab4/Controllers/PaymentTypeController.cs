using lab4.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Controllers
{
    public class PaymentTypeController : Controller
    {
        private UtilitiesContext _context;
        public PaymentTypeController(UtilitiesContext context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 260, Location = ResponseCacheLocation.Client)]
        public IActionResult Index()
        {
            var indications = _context.PaymentTypes;
            return View(indications);
        }
    }
}
