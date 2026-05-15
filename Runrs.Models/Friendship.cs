using Runrs_SD7501.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public int RequesterId { get; set; }   // who sent request
        public int AddresseeId { get; set; }   // who receives request

        public DateTime CreatedAt { get; set; }

        public FriendshipStatus Status { get; set; }

        public User Requester { get; set; }
        public User Addressee { get; set; }
    }

    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
