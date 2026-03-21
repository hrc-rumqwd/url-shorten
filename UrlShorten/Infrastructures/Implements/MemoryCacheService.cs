using Microsoft.Extensions.Caching.Memory;
using UrlShorten.Infrastructures.Abstracts;

namespace UrlShorten.Infrastructures.Implements
{
    public class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
        public T Add<T>(string key, T value)
        {
            return cache.Set(key, value);
        }

        public bool Delete(string key)
        {
            cache.Remove(key);
            return true;
        }

        public T? Get<T>(string key)
        {
            return cache.Get<T>(key);
        }
    }
}
