using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runrs.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        [ValidateNever]
        public RunEvent? Event { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }
    }
}
