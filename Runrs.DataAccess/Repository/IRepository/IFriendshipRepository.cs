using Runrs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IFriendshipRepository : IRepository<Friendship>
    {
        IEnumerable<Friendship> GetFriends(int userId);
        IEnumerable<Friendship> GetPendingRequests(int userId);
    }
}
