using lab6.EF;
using lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab6.Controllers
{
    [Route("api/Indication")]
    public class IndicationController : Controller
    {
        private UtilitiesContext _context;
        public IndicationController(UtilitiesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Indication> GetAll()
        {
            return _context.Indications.Include(i => i.Counter).AsEnumerable();
        }

        [HttpGet("{id}")]
        public Indication? Get(int id)
        {
            return _context.Indications.Include(i => i.Counter).FirstOrDefault(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Indication? indication = _context.Indications.FirstOrDefault(e => e.Id == id);
            if (indication == null)
            {
                return NotFound();
            }
            else
            {
                _context.Indications.Remove(indication);
                _context.SaveChanges();
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] IndicationForCreate indicationForCreate)
        {
            if (indicationForCreate == null || indicationForCreate.ApartmentId == 0 ||
                indicationForCreate.Date == DateTime.MinValue|| indicationForCreate.Volume == 0 || indicationForCreate.PaymentTypeId == 0)
                return BadRequest();
            else
            {
                var indication = new Indication()
                {
                    Date = indicationForCreate.Date,
                    Volume = indicationForCreate.Volume,
                    ApartmentId = indicationForCreate.ApartmentId,
                    CounterId = _context.Counters.FirstOrDefault(c => c.PaymentTypeId == indicationForCreate.PaymentTypeId).Id
                };
                _context.Indications.Add(indication);
                _context.SaveChanges();
                return Ok(indication);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] IndicationForUpdate indicationForUpdate)
        {
            if (_context.Indications.Count(e => e.Id == indicationForUpdate.Id) == 0)
                return BadRequest();
            else
            {
                var indication = new Indication()
                {
                    Id = indicationForUpdate.Id,
                    Date = indicationForUpdate.Date,
                    Volume = indicationForUpdate.Volume,
                    ApartmentId = indicationForUpdate.ApartmentId,
                    CounterId = _context.Counters.FirstOrDefault(c => c.PaymentTypeId == indicationForUpdate.PaymentTypeId).Id
                };
                _context.Indications.Update(indication);
                _context.SaveChanges();
                return Ok(indication);
            }
        }
    }
}
