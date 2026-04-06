using Runrs_SD7501.Models;
using Microsoft.EntityFrameworkCore;

namespace Runrs_SD7501.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Membership> Memberships { get; set; }
    }
}
