using lab3.Utilities.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace lab3.Utilities.Services
{
    public class CachedApartmentsService
    {
        private UtilitiesContext _db;
        private IMemoryCache _cache;
        private int _rowsNumber;

        public CachedApartmentsService(UtilitiesContext context, IMemoryCache memoryCache)
        {
            _db = context;
            _cache = memoryCache;
            _rowsNumber = 20;
        }

        public IEnumerable<Apartment> GetApartment()
        {
            return _db.Apartments.Take(_rowsNumber).ToList();
        }

        public void AddApertments(string cacheKey)
        {
            IEnumerable<Apartment> apertments = _db.Apartments.Take(_rowsNumber);

            _cache.Set(cacheKey, apertments, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public IEnumerable<Apartment> GetApartments(string cacheKey)
        {
            IEnumerable<Apartment> apartments = null;
            if (!_cache.TryGetValue(cacheKey, out apartments))
            {
                apartments = _db.Apartments.Take(_rowsNumber).ToList();
                if (apartments != null)
                {
                    _cache.Set(cacheKey, apartments,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(260)));
                }
            }
            return apartments;
        }

    }
}
