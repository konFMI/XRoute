using Microsoft.EntityFrameworkCore;
using XRoute.Models;

namespace XRoute.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
    }
}