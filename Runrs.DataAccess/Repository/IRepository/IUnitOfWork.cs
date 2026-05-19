using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runrs.DataAccess.Data;

namespace Runrs.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IClubRepository Club { get; }
        IUserRepository User { get; }
        IMembershipRepository Membership { get; }
        IFriendshipRepository Friendship { get; }
        IEventRepository Event { get; } // <----- Added IEventRepository to UnitOfWork (Byron 16/05/2026)
        IEventRegistrationRepository EventRegistration { get; } // <----- Added IEventRegistrationRepository to UnitOfWork (Byron 16/05/2026)
        void Save();
    }
}
