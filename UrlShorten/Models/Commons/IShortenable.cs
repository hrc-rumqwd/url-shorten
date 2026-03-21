namespace UrlShorten.Models.Commons
{
    public interface IShortenable
    {
        string ShortenHash(long value);
    }
}
