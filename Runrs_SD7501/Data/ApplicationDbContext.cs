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
                },
            // Byron 17/04/2026 - NEW Seed Data for Test Users
                new User
                {
                    Id = 2,
                    Username = "testuser2", // <-- Username for logging in with Test User
                    FirstName = "Test2",
                    LastName = "User2",
                    Email = "Test2@gmail.com",
                    PasswordHash = "Test123", // <-- Password for logging in with Test User
                    DateOfBirth = new DateTime(1999, 3, 7),
                    JoinedAt = new DateTime(2026, 4, 17)
                },
                new User
                {
                    Id = 3,
                    Username = "testuser3", // <-- Username for logging in with Test User
                    FirstName = "Test3",
                    LastName = "User3",
                    Email = "Test@gmail.com",
                    PasswordHash = "Test123", // <-- Password for logging in with Test User
                    DateOfBirth = new DateTime(1999, 3, 7),
                    JoinedAt = new DateTime(2026, 4, 17),
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
                },
            // Byron 17/04/2026 - NEW Seed Data for Test Clubs
                new Club
                {
                    Id = 2,
                    ClubName = "Bay Runners",
                    ClubDescription = "Wanting a challenge? Join our run club that regularly does the famous 'Bays Route', a 30km scenic route along some of the most beautiful bays Wellington has to offer.",
                    ClubLocation = "Wellington CBD, Wellington",
                    IsPrivate = false,
                    CreatedAt = new DateTime(2026, 4, 17),
                    OwnerId = 2,
                    ImageUrl = ""
                },
                new Club
                {
                    Id = 3,
                    ClubName = "Social Runners WLG",
                    ClubDescription = "Join our social run club based in Porirua which is open to all levels of fitness. We meet every Saturday at the Porirua pools to complete a 5km run and socialise over coffee after. ",
                    ClubLocation = "Porirua, Wellington",
                    IsPrivate = false,
                    CreatedAt = new DateTime(2026, 4, 17),
                    OwnerId = 3,
                    ImageUrl = ""
                }
            );
            // ------------------------------------------
        }
    }
}
