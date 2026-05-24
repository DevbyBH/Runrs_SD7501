using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Announcement Content Required")]
        [MaxLength(1000)]
        [DisplayName("Announcement")]
        public string Content { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public int ClubId { get; set; }

        [Required]
        public int PostedByUserId { get; set; }

        [ForeignKey("ClubId")]
        public Club? Club { get; set; }

        [ForeignKey("PostedByUserId")]
        public User? PostedBy { get; set; }
    }
}
