namespace UrlShorten.Infrastructures.Abstracts
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        T Add<T>(string key, T value);
        bool Delete(string key);
    }
}
