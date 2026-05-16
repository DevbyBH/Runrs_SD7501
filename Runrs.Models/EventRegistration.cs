using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Runrs.Models;

namespace Runrs.Models
{
    public class EventRegistration // <----- Event Registration Model Class (Byron 16/05/2026)
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Confirmed; // Status of the Registration (Registered, Cancelled, Waitlisted)

        [Required]
        [DisplayName("PaymentStatus:")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid; // Status of the Payment (Unpaid, Paid)

        [Required]
        public int UserId { get; set; } // Foreign Key to the User who registered

        [ForeignKey("UserId")]
        public int EventId { get; set; } // Nav to the User who registered

        [Required]
        public int RunEventId { get; set; } // Foreign Key to the Event being registered for

        [ForeignKey("RunEventId")]
        public RunEvent? RunEvent { get; set; } // Nav to the Event being registered for

        [ForeignKey("UserId")]
        public User? User { get; set; } // Nav to the User who registered

        [Display(Name = "Registration Date:")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now; // Date of Registration
    }

    public enum RegistrationStatus
    {
        Confirmed,
        Cancelled,
        Waitlisted
    }

    public enum PaymentStatus
    {
        Unpaid,
        Paid
    }
}
