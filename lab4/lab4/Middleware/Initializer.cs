using lab4.Models;

namespace lab4.Middleware
{
    public class Initializer
    {
        private readonly RequestDelegate _next;

        public Initializer(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, UtilitiesContext dbcontext)
        {
            GetPaymentTypes(dbcontext);
            GetApartments(dbcontext);
            GetCounters(dbcontext);
            GetIndications(dbcontext);
            GetRates(dbcontext);
            return _next(httpContext);
        }

        private void GetPaymentTypes(UtilitiesContext context)
        {
            if (!context.PaymentTypes.Any())
            {
                var paymentTypes = new PaymentType[3];
                paymentTypes[0] = new PaymentType() { Name = "газ" };
                paymentTypes[1] = new PaymentType() { Name = "вода" };
                paymentTypes[2] = new PaymentType() { Name = "электричество" };

                context.PaymentTypes.AddRange(paymentTypes);
                context.SaveChanges();
            }
        }
        private void GetApartments(UtilitiesContext context)
        {
            if (!context.Apartments.Any())
            {
                var apartments = new Apartment[50];
                var random = new Random();

                for (int i = 0; i < apartments.Length; i++)
                {
                    apartments[i] = new Apartment()
                    {
                        FirstName = "Имя " + i,
                        LastName = "Фамилия " + i,
                        MiddleName = "Отчество " + i,
                        HumanCount = random.Next(1, 7),
                        Square = random.Next(10, 100)
                    };
                }

                context.Apartments.AddRange(apartments);
                context.SaveChanges();
            }
        }
        private void GetCounters(UtilitiesContext context)
        {
            if (!context.Counters.Any())
            {
                var counters = new Counter[50];
                var random = new Random();
                for (int i = 0; i < counters.Length; i++)
                {
                    counters[i] = new Counter { PaymentTypeId = context.PaymentTypes.Skip(random.Next(0, context.PaymentTypes.Count())).First().Id };
                }
                context.Counters.AddRange(counters);
                context.SaveChanges();
            }
        }
        private void GetIndications(UtilitiesContext context)
        {
            if (!context.Indications.Any())
            {
                var indications = new Indication[1000];
                var random = new Random();
                for (int i = 0; i < indications.Length; i++)
                {
                    indications[i] = new Indication()
                    {
                        ApartmentId = context.Apartments.Skip(random.Next(0, context.Apartments.Count())).First().Id,
                        Volume = random.Next(1, 50),
                        Date = new DateTime(2023, random.Next(1, 13), random.Next(1, 28)),
                        CounterId = context.Counters.Skip(random.Next(0, context.Counters.Count())).First().Id
                    };
                }
                context.Indications.AddRange(indications);
                context.SaveChanges();
            }

        }
        private void GetRates(UtilitiesContext context)
        {
            if (!context.Rates.Any())
            {
                var random = new Random();
                var rates = new Rate[50];
                for (int i = 0; i < rates.Length; i++)
                {
                    rates[i] = new Rate()
                    {
                        Date = new DateTime(2023, random.Next(1, 13), random.Next(1, 28)),
                        Price = random.Next(1, 50),
                        PaymentTypeId = context.PaymentTypes.Skip(random.Next(0, context.PaymentTypes.Count())).First().Id
                    };
                }
                context.Rates.AddRange(rates);
                context.SaveChanges();
            }
        }
    }
}
