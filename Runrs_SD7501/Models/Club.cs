using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Runrs_SD7501.Models
{
    public class Club
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Club Name is Required")]
        [MaxLength(100)]
        [DisplayName("Club Name")]
        public string ClubName { get; set; }

        [Required(ErrorMessage = "Club Description is Required")]
        [MaxLength(300)]
        [DataType(DataType.MultilineText)]
        public string ClubDescription { get; set; }

        [Required(ErrorMessage = "Club Location is Required")]
        [MaxLength(100)]
        [DisplayName("Club Location")]
        public string ClubLocation { get; set; }

        [Display(Name = "Private Club")]
        public bool IsPrivate { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
