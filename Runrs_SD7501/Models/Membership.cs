using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runrs_SD7501.Models
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Role Required")]
        [RegularExpression("Owner|Member", ErrorMessage = "Role must be Owner or Member")]
        public string Role { get; set; } = "Member";

        [Display(Name = "Joined On")]
        [DataType(DataType.DateTime)]
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        [Required]
        [EnumDataType(typeof(MembershipStatus), ErrorMessage = "Invalid membership status")]
        public MembershipStatus Status { get; set; } = MembershipStatus.Pending;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ClubId")]
        public Club? Club { get; set; }
    }
    public enum MembershipStatus
    {
        Pending, Approved, Rejected
    }
}
