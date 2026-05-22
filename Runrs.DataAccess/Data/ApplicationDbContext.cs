using Runrs.Models;
using Microsoft.EntityFrameworkCore;
using Runrs.DataAccess.Repository;
using Runrs.DataAccess.Data;
using Runrs.DataAccess;
using static Runrs.Models.Club;

namespace Runrs.DataAccess.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<RunEvent> RunEvents { get; set; } // <----- DbSet for RunEvent (Byron 16/05/2026)
        public DbSet<EventRegistration> EventRegistrations { get; set; } // <----- DbSet for EventRegistration (Byron 16/05/2026)


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

            modelBuilder.Entity<EventRegistration>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventRegistration>()
                .HasOne(e => e.RunEvent)
                .WithMany(r => r.Registrations)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>() // <---- Added configuration for Mo's Friendship entity to stop deleting related issues with seeded data (Byron 16/05/2026)
                .HasOne(f => f.Requester)
                .WithMany()
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Addressee)
                .WithMany()
                .HasForeignKey(f => f.AddresseeId)
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
                    Difficulty = DifficultyLevel.Intermediate, // <- - Byron 17/04/2026 - Edited to match new DifficultyLevel enum
                    Distance = DistanceRange.FiveToTen, // <- - Byron 17/04/2026 - Edited to match new DistanceRange enum
                    Type = ClubType.Casual, // <- - Byron 17/04/2026 - Edited to match new ClubType enum
                    OwnerId = 1,
                    ImageUrl = "https://wordpress.nzrunning.co.nz/wp-content/uploads/2025/04/445cover.jpg"
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
                    Difficulty = DifficultyLevel.Advanced, // <- - Byron 17/04/2026 - Edited to match new DifficultyLevel enum
                    Distance = DistanceRange.TenPlus, // <- - Byron 17/04/2026 - Edited to match new DistanceRange enum
                    Type = ClubType.Training, // <- - Byron 17/04/2026 - Edited to match new ClubType enum
                    OwnerId = 2,
                    ImageUrl = "https://www.changefitness.co.nz/wp-content/uploads/feb78d35-680f-4f15-81c1-1e7a0183f311.jpg"
                },
                new Club
                {
                    Id = 3,
                    ClubName = "Social Runners WLG",
                    ClubDescription = "Join our social run club based in Porirua which is open to all levels of fitness. We meet every Saturday at the Porirua pools to complete a 5km run and socialise over coffee after. ",
                    ClubLocation = "Porirua, Wellington",
                    IsPrivate = false,
                    CreatedAt = new DateTime(2026, 4, 17),
                    Difficulty = DifficultyLevel.Beginner, // <- - Byron 17/04/2026 - Edited to match new DifficultyLevel enum
                    Distance = DistanceRange.FiveToTen, // <- - Byron 17/04/2026 - Edited to match new DistanceRange enum
                    Type = ClubType.Social, // <- - Byron 17/04/2026 - Edited to match new ClubType enum
                    OwnerId = 3,
                    ImageUrl = "https://cdn.eventfinda.co.nz/uploads/events/transformed/1771695-766708-34.jpg"
                }
            );
            // ------------------------------------------

            modelBuilder.Entity<RunEvent>().HasData( // <----- Seed Data for RunEvents (Byron 16/05/2026)
                new RunEvent
                {
                    Id = 1,
                    EventTitle = "Petone Run For Fun",
                    EventDescription = "Join us for a run along Petone Esplanade. This event will be an introductory event for any new members wanting to come and feel out the club!",
                    EventDate = new DateTime(2026, 11, 15, 9, 0, 0),
                    EventLocation = "Petone Esplanade, Wellington",
                    Distance = 10,
                    MaxParticipants = 20,
                    EntryFee = 0,
                    CreatedAt = new DateTime(2026, 4, 17),
                    ClubId = 1
                },
                new RunEvent
                {
                    Id = 2,
                    EventTitle = "Bays Route Marathon",
                    EventDescription = "The annual BAYS ROUTE MARATHON is coming up. Join us to raise money for local charities in Wellington.",
                    EventDate = new DateTime(2026, 11, 22, 8, 0, 0),
                    EventLocation = "Wellington Waterfront, Wellington",
                    Distance = 30,
                    MaxParticipants = 50,
                    EntryFee = 20,
                    CreatedAt = new DateTime(2026, 4, 17),
                    ClubId = 2
                },
                new RunEvent
                {
                    Id = 3,
                    EventTitle = "Porirua 5km Social Run",
                    EventDescription = "Our weekly Saturday 5km social run followed by coffee.",
                    EventDate = new DateTime(2026, 6, 6, 9, 0, 0),
                    EventLocation = "Porirua Pools, Porirua",
                    Distance = 5.0,
                    MaxParticipants = 50,
                    EntryFee = 0,
                    CreatedAt = new DateTime(2026, 4, 17),
                    ClubId = 3
                }
            );

           // mo 15/05/26 - stops cascade delete paths
            modelBuilder.Entity<Friendship>()
                 .HasOne(f => f.Requester)
                 .WithMany()
                 .HasForeignKey(f => f.RequesterId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Addressee)
                .WithMany()
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
