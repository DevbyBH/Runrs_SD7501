using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.Models.ViewModels
{
    public class ProfileVM
    {
        public User User { get; set; }
        public int CurrentUserId { get; set; }
        public Friendship? Friendship { get; set; }
        public List<Friendship> Friends { get; set; } = new();
    }
}
