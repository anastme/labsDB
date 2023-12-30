using lab6.EF;
using Microsoft.AspNetCore.Mvc;

namespace lab6.Controllers
{
    [Route("api/Apartment")]
    public class ApartmentController : Controller
    {
        private UtilitiesContext _context;
        public ApartmentController(UtilitiesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Apartment> Get()
        {
            return _context.Apartments.AsEnumerable();
        }

        [HttpGet("{id}")]
        public Apartment? Get(int id)
        {
            return _context.Apartments.FirstOrDefault(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Apartment? apartment = _context.Apartments.FirstOrDefault(e => e.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }
            else
            {
                _context.Apartments.Remove(apartment);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] Apartment apartment)
        {
            if (apartment == null || apartment.FirstName == null || apartment.LastName == null
                || apartment.MiddleName == null || apartment.Square == 0 || apartment.HumanCount == 0)
                return BadRequest();
            else
            {
                _context.Apartments.Add(apartment);
                _context.SaveChanges();
                return Ok(apartment);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] Apartment apartment)
        {
            if (_context.Apartments.Count(e => e.Id == apartment.Id) == 0)
                return BadRequest();
            else
            {
                _context.Apartments.Update(apartment);
                _context.SaveChanges();
                return Ok(apartment);
            }
        }
    }
}
