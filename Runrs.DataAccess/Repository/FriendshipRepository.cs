using Microsoft.EntityFrameworkCore;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;
using Runrs_SD7501.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository
{
    public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationDbContext _db;

        public FriendshipRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Friendship> GetFriends(int userId)
        {
            return _db.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Where(f =>
                    (f.RequesterId == userId || f.AddresseeId == userId) &&
                    f.Status == FriendshipStatus.Accepted)
                .ToList();
        }

        public IEnumerable<Friendship> GetPendingRequests(int userId)
        {
            return _db.Friendships
                .Include(f => f.Requester)
                .Where(f =>
                    f.AddresseeId == userId &&
                    f.Status == FriendshipStatus.Pending)
                .ToList();
        }
    }
}
