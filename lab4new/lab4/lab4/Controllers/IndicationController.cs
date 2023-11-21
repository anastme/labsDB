using lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab4.Controllers
{
    public class IndicationController : Controller
    {
        private UtilitiesContext _context;
        public IndicationController(UtilitiesContext context)
        {
            _context = context;
        }
        [ResponseCache(Duration = 260, Location = ResponseCacheLocation.Client)]
        public IActionResult Index()
        {
            var indications = _context.Indications.Include(i => i.Apartment).Include(i => i.Counter).ThenInclude(c => c.PaymentType);
            return View(indications);
        }
    }
}
