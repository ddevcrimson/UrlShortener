using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace UrlShortener.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Link> Links { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
