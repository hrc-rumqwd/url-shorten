using Microsoft.EntityFrameworkCore;
using UrlShorten.Models.Entities;

namespace UrlShorten.Persistences
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShortenLink> ShortenLinks { get; set; }
    }
}
