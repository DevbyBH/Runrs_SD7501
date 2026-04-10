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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Membership>()
                .HasOne(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Membership>()
                .HasOne(m => m.Club)
                .WithMany(c => c.Memberships)
                .HasForeignKey(m => m.ClubId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
