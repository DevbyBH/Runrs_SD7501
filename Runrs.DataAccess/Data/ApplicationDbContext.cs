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
        public DbSet<Announcement> Announcements { get; set; } // <----- DbSet for Announcements (Byron 17/05/2026)
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } // <----- DbSet for ShoppingCart (Byron 17/05/2026)


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

            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.PostedBy)
                .WithMany()
                .HasForeignKey(a => a.PostedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.Club)
                .WithMany()
                .HasForeignKey(a => a.ClubId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.Event)
                .WithMany()
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
