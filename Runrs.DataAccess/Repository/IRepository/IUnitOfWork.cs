using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IClubRepository Club { get; }
        IUserRepository User { get; }
        IMembershipRepository Membership { get; }
        IFriendshipRepository Friendship { get; }
        void Save();
    }
}
