using UrlShorten.Models.Commons;

namespace UrlShorten.Models.Entities
{
    public class ShortenLink : BaseEntity<long>
    {
        public string SourceLink { get; set; }
        public string ShortenPath { get; set; }

        public static ShortenLink Create(string sourceLink, string shortenPath)
            => new ShortenLink
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System",
                ShortenPath = shortenPath,
                SourceLink = sourceLink,
            };
    }
}
