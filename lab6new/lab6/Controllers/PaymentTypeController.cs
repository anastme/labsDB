using lab6.EF;
using Microsoft.AspNetCore.Mvc;

namespace lab6.Controllers
{
    [Route("api/PaymentType")]
    public class PaymentTypeController
    {
        private UtilitiesContext _context;

        public PaymentTypeController(UtilitiesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<PaymentType> GetAll()
        {
            return _context.PaymentTypes.AsEnumerable();
        }
    }
}
