using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Runrs_SD7501.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(100)]
        [DisplayName("First Name")]
        public string FirstName{ get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(100)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is Required")]
        [MaxLength(100)]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime JoinedAt { get; set; }
    }
}
