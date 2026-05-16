using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Runrs.Models
{
    public class RunEvent // <----- Event Model Class (Byron 16/05/2026)
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "Event Title Is Required!")]
        [MaxLength(100)]
        [DisplayName("Event Title:")]
        public string? EventTitle { get; set; } // Name of Event

        [Required(ErrorMessage = "Event Description Is Required!")]
        [MaxLength(500)]
        [DisplayName("Event Description:")]
        public string? EventDescription { get; set; } // Description of the Event

        [Required(ErrorMessage = "Event Date Is Required!")]
        [DataType(DataType.DateTime)]
        [DisplayName("Event Date:")]
        public DateTime EventDate { get; set; } // Date of the Event

        [Required(ErrorMessage = "Event Location Is Required!")]
        [MaxLength(100)]
        [DisplayName("Event Location:")]
        public string? EventLocation { get; set; } // Location of the Event

        [Required(ErrorMessage = "Run Distance Is Required!")]
        [Range(0.1, 500.0, ErrorMessage = "Distance must be between 0.1 and 500 km")]
        [DisplayName("Event Run Distance:")]
        public double? Distance { get; set; } // Distance of the Run Event

        [Required(ErrorMessage = "Max Participants is Required")]
        [Range(1, 10000, ErrorMessage = "Must be between 1 and 10,000")]
        [DisplayName("Max Participants:")]
        public int? MaxParticipants { get; set; } // Maximum Participants allowed for the Event

        [DisplayName("Entry Fee:")]
        [Range(0, 1000, ErrorMessage = "Event Fee must be between $0 and $1000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EntryFee { get; set; } = 0; // Entry Fee for the Event ($0 for free events)

        [Display(Name = "Created on:")]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Date the Event was created

        [Required]
        public int ClubId { get; set; } // Foreign Key to the Club that is hosting the Event

        [ForeignKey("ClubId")]
        public Club? Club { get; set; } // Nav to the Club hosting the Event

        public ICollection<EventRegistration>? Registrations { get; set; }

    }
}
