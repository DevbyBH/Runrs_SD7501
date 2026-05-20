using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.Models.ViewModels
{
    public class ActivityItem
    {
        public string Message { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string? Url { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
