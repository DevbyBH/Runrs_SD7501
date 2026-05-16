using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runrs.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IClubRepository Club { get; private set; }
        public IUserRepository User { get; private set; }
        public IMembershipRepository Membership { get; private set; }
        public IFriendshipRepository Friendship { get; private set; }
        public IEventRepository Event { get; private set; } // <----- Added IEventRepository property to UnitOfWork (Byron 16/05/2026)
        public IEventRegistrationRepository EventRegistration { get; private set; } // <----- Added IEventRegistrationRepository property to UnitOfWork (Byron 16/05/2026)

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Club = new ClubRepository(_db);
            User = new UserRepository(_db);
            Membership = new MembershipRepository(_db);
            Friendship = new FriendshipRepository(_db);
            Event = new EventRepository(_db);
            EventRegistration = new EventRegistrationRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
