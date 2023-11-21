using lab4.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Controllers
{
    public class ApartmentController : Controller
    {
        private UtilitiesContext _context;

        public ApartmentController(UtilitiesContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 260, Location = ResponseCacheLocation.Client)]
        public IActionResult Index()
        {
            var apartments = _context.Apartments;
            return View(apartments);
        }
    }
}
