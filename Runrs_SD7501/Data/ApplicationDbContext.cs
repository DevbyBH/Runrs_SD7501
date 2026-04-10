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
            // Byron 10/04/2026 - Configured the relationships between User, Club, and Membership entities to stop deleting related issues with seeded data
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
            // --------------------------------------------------------------------------------------------------------------------------------------------

            // Byron 10/04/2026 - Seed Data for Test User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "testuser", // <-- Username for logging in with Test User
                    FirstName = "Test",
                    LastName = "User",
                    Email = "Test@gmail.com",
                    PasswordHash = "Test123", // <-- Password for logging in with Test User
                    DateOfBirth = new DateTime(1999, 3, 7),
                    JoinedAt = new DateTime(2026, 10, 4)
                }
            );
            // ------------------------------------------

            // Byron 10/04/2026 - Seed Data for Test Club
            modelBuilder.Entity<Club>().HasData(
                new Club
                {
                    Id = 1,
                    ClubName = "Hutt Valley Run Club",
                    ClubDescription = "Join us every Wednesday & Saturday for a 10km run along Petone Esplanade/Beach",
                    ClubLocation = "Petone, Wellington",
                    IsPrivate = false,
                    CreatedAt = new DateTime(2026, 10, 4),
                    OwnerId = 1,
                    ImageUrl = ""
                }
            );
            // ------------------------------------------
        }
    }
}
