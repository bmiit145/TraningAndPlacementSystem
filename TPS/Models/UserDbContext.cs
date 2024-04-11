using Microsoft.EntityFrameworkCore;

namespace TPS.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<Hiring> Users { get; set; }
    }
}
