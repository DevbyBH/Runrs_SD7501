using Runrs.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Runrs.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public ICollection<Membership>? Memberships { get; set; }
        public ICollection<Club>? OwnedClubs { get; set; }
    }
}