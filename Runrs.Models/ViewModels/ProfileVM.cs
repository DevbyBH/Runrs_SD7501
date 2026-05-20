using System;
using System.Collections.Generic;

namespace Runrs.Models.ViewModels
{
    public class ProfileVM
    {
        public User User { get; set; }

        public int CurrentUserId { get; set; }

        public Friendship? Friendship { get; set; }

        public List<Friendship> Friends { get; set; } = new();

        public List<ActivityItem> Activities { get; set; } = new();
    }
}