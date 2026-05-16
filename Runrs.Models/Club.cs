using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runrs.Models
{
    public class Club
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Club Name is Required")]
        [MaxLength(100)]
        [DisplayName("Club Name:")]
        public string ClubName { get; set; }

        [Required(ErrorMessage = "Club Description is Required")]
        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Club Description:")]
        public string ClubDescription { get; set; }

        [Required(ErrorMessage = "Club Location is Required")]
        [MaxLength(100)]
        [DisplayName("Club Location:")]
        public string ClubLocation { get; set; }

        [Required]
        [DisplayName("Difficulty Level:")]
        public DifficultyLevel Difficulty { get; set; } // <-- Byron 17/04/2026 Add DifficultyLevel enum

        [Required]
        [DisplayName("Distance Range:")]
        public DistanceRange Distance { get; set; } // <-- Byron 17/04/2026 Add DistanceRange enum

        [Required]
        [DisplayName("Club Type:")] // <-- Byron 17/04/2026 Add ClubType enum
        public ClubType Type { get; set; }

        [Display(Name = "Private Club?:")]
        public bool IsPrivate { get; set; } = false;

        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }

        [DisplayName("Club Image URL:")]
        public string? ImageUrl { get; set; }


        public ICollection<Membership>? Memberships { get; set; }
        public enum DistanceRange // <------ Byron 17/04/2026 Add DistanceRange enum
        {
            [Display(Name = "1-5km")]
            OneToFive,
            [Display(Name = "5-10km")]
            FiveToTen,
            [Display(Name = "10+km")]
            TenPlus
        }
        public enum DifficultyLevel // <------ Byron 17/04/2026 Add DifficultyLevel enum
        {
            Beginner, Intermediate, Advanced
        }
        public enum ClubType // <------ Byron 17/04/2026 Add ClubType enum
        {
            Casual, Competitive, Social, Training, Other
        }
    }
}
