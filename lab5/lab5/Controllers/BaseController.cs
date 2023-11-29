using lab5.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace lab5.Controllers
{
    public class BaseController : Controller
    {
        protected IMemoryCache _cache;
        protected UtilitiesContext _context;

        protected int _cacheTime = 360;

        public BaseController(UtilitiesContext context, IMemoryCache cache)
        {
            _cache = cache;
            _context = context;
        }

        protected void CacheClear()
        {
            MethodInfo clearMethod = _cache.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
            if (clearMethod != null)
            {
                clearMethod.Invoke(_cache, null);
                return;
            }
            else
            {
                PropertyInfo prop = _cache.GetType().GetProperty("EntriesCollection", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public);
                if (prop != null)
                {
                    object innerCache = prop.GetValue(_cache);
                    if (innerCache != null)
                    {
                        clearMethod = innerCache.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
                        if (clearMethod != null)
                        {
                            clearMethod.Invoke(innerCache, null);
                            return;
                        }
                    }
                }
            }
        }
        protected string GetStringFromSession(HttpContext context, string key, string queryName, string defaultValue = "")
        {
            if (context.Request.Query[queryName].Count() > 0)
            {
                return context.Request.Query[queryName][0];
            }
            else if (context.Session.GetString(key) != null)
            {
                return context.Session.GetString(key);
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
