using UrlShorten.Models.Commons;

namespace UrlShorten.Infrastructures.Abstracts
{
    public interface IShortenService
    {
        Task<Result<string>> ShortenUrlAsync(string url);
        Task<Result<string>> GetShortenPureValueAsync(string encodedPath);
    }
}
