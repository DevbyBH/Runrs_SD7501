using Microsoft.EntityFrameworkCore;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.DataAccess.Data;

namespace Runrs.DataAccess.Repository
{
    public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
    {                                                           
        private readonly ApplicationDbContext _db;

        public FriendshipRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Friendship obj)
        {
            _db.Friendships.Update(obj);
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

        public bool AreFriends(int userId1, int userId2)
        {
            return _db.Friendships.Any(f =>
                ((f.RequesterId == userId1 && f.AddresseeId == userId2) ||
                 (f.RequesterId == userId2 && f.AddresseeId == userId1)) &&
                 f.Status == FriendshipStatus.Accepted);
        }
    }
}
