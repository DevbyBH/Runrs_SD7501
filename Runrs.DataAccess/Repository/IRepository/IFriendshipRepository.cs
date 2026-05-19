using Runrs.Models;
using System;
using System.Collections.Generic;
using Runrs.DataAccess.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IFriendshipRepository : IRepository<Friendship>
    {
        void Update(Friendship obj);

        IEnumerable<Friendship> GetFriends(int userId);
        IEnumerable<Friendship> GetPendingRequests(int userId);
        bool AreFriends(int userId1, int userId2);
    }
}
