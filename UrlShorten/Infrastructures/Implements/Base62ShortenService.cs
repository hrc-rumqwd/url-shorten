using Microsoft.EntityFrameworkCore;
using System.Text;
using UrlShorten.Models.Commons;
using UrlShorten.Models.Entities;
using UrlShorten.Persistences;
using UrlShorten.Infrastructures.Abstracts;

namespace UrlShorten.Infrastructures.Implements
{
    public class Base62ShortenService : IShortenService, IShortenable
    {
        private const string Base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly ApplicationDbContext _context;
        private readonly string _hostDomain;
        private readonly ICacheService _cache;
        private readonly IRabbitMqBus _messageBus;

        private const string UpdateShortenPathQueue = "shorten-link.update";

        public Base62ShortenService(
            ApplicationDbContext context,
            IConfiguration configuration,
            ICacheService cache,
            IRabbitMqBus messageBus)
        {
            _context = context;
            _hostDomain = configuration["Host"];
            _cache = cache;
            _messageBus = messageBus;
        }

        public async Task<Result<string>> ShortenUrlAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }

            StringBuilder sb = new();
            sb.Append(_hostDomain);

            var shorten = ShortenLink.Create(url, string.Empty);
            _context.ShortenLinks.Add(shorten);
            await _context.SaveChangesAsync();

            // Encoding the ID to Base62
            string encodedPath = ShortenHash(shorten.Id);
            shorten.ShortenPath = encodedPath;

            // Firing event to message broker here with shorten.Id and encodedPath to update shorten path in database
            await _messageBus.PublishAsync(shorten, UpdateShortenPathQueue);

            // Caching value
            _cache.Add(encodedPath, shorten.SourceLink);

            sb.Append(encodedPath);
            return Result<string>.Success(sb.ToString());
        }


        public async Task<Result<string>> GetShortenPureValueAsync(string encodedPath)
        {
            // Searching in cache first
            string? url = _cache.Get<string>(encodedPath);

            if(!string.IsNullOrEmpty(url))
                return Result<string>.Success(url);

            // Found in database if not found 
            var item = await _context.ShortenLinks
                .Where(x => x.ShortenPath == encodedPath)
                .FirstOrDefaultAsync();

            return item is null
                ? Result<string>.Failure("Not found matching shorten link")
                : Result<string>.Success(item.SourceLink);
        }

        #region Built-in Shorten algorithm
        public string ShortenHash(long value)
        {
            string tinyPath = string.Empty;

            if (value > 0)
            {
                tinyPath += ShortenHash(value / 62) + Base62Chars[(int)(value % 62)].ToString();
                return tinyPath;
            }

            return string.Empty;
        }
        #endregion
    }
}
